using AutoMapper;
using Google.Apis.Auth;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Entities;
using LearnProject.Domain.Repositories;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LearnProject.BLL.Services.Services
{
    /// <summary>
    /// сервис аутентификации
    /// </summary>
    public class IdentityService : IIdentityService
    {
        readonly IRepositoryWrapper repository;
        readonly IMapper mapper;
        readonly ILogger<IdentityService> logger;
        readonly IOptions<JwtSettings> jwtSettings;
        readonly ExternalProviders providers;
        readonly IConfiguration configuration;
        readonly UserManager<User> userManager;

        public IdentityService(IRepositoryWrapper repository, IMapper mapper, ILogger<IdentityService> logger,
            IOptions<JwtSettings> jwtSetttings,
            UserManager<User> userManager, IConfiguration configuration)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
            this.jwtSettings = jwtSetttings;
            this.userManager = userManager;
            this.configuration = configuration;
        }

        /// <summary>
        /// вход в приложение
        /// </summary>
        /// <param name="model">модель данных для входа</param>
        /// <returns>ответ с моделью AuthenticationResult</returns>
        public async Task<AuthenticationResponse> LogInAsync(LoginUserModel model)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);

            if (existingUser == null)
            {
                logger.LogInformation("Cannot login user {Email}.", model.Email);
                return  AuthenticationResponse.CreateFailedResponse(AuthenticationResult.UserDoesNotExist, new List<string> { "invalid login or password" });
            }

            var checkPassword = await userManager.CheckPasswordAsync(existingUser, model.Password);

            if (!checkPassword)
            {
                logger.LogInformation("Cannot login user {Email}.", model.Email);
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.InvalidPasswordWhileLogin, new List<string> { "invalid login or password" });
            }

            var role = (await repository.UserRepository.ReadWithRoleAsync(existingUser.Id)).Role;

            IDictionary<string, object> claims = new Dictionary<string, object>
            {
                { "role", role }
            };

            logger.LogInformation("the user {Email} is logged into the application.", existingUser.Email);

            return await CreateAuthenticationResultAsync(existingUser, claims);
        }

        public async Task<AuthenticationResponse> LogInWithGoogleAsync(string token)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = new List<string> { providers.google.ClientId }
            };

            GoogleJsonWebSignature.Payload payload;
            try
            {
                payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            }
            catch (InvalidJwtException e)
            {
                logger.Log(LogLevel.Information, e.Message);
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.TokenValidationFailed,
                    new List<string> { "Google authentication failed - invalidToken" });
            }

            var existingUser = await userManager.FindByEmailAsync(payload.Email);

            if (existingUser != null) 
            {
                var role = (await repository.UserRepository.ReadWithRoleAsync(existingUser.Id)).Role;

                IDictionary<string, object> claims = new Dictionary<string, object>
                {
                    { "role", role }
                };

                logger.Log(LogLevel.Information, "Google authentication succeded");

                return await CreateAuthenticationResultAsync(existingUser, claims);
            }

            User newUser = new User()
            {
                Name = payload.GivenName,
                Surname = payload.FamilyName,
                Email = payload.Email
            };

            var result = await userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                logger.LogInformation("Cannot register user {Email}.", payload.Email);
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.RegistrationFailed, result.Errors.Select(error => error.Description).ToList());
            }

            await userManager.AddToRoleAsync(newUser, AppRoles.User);

            IDictionary<string, object> newUserClaims = new Dictionary<string, object>
            {
                { "role", AppRoles.User }
            };

            logger.Log(LogLevel.Information, "Google authentication succeded");

            return await CreateAuthenticationResultAsync(newUser, newUserClaims);
        }

        /// <summary>
        /// регистрация пользователя
        /// </summary>
        /// <param name="model">модель добавления пользователя</param>
        /// <returns>ответ с моделью AuthenticationResult</returns>
        public async Task<AuthenticationResponse> RegisterAsync(RegisterUserModel model)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                logger.LogInformation("Cannot register user {Email}.", model.Email);
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.UserAlreadyExists, new List<string> { "user already exists" });
            }

            User user = mapper.Map<User>(model);

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                logger.LogInformation("Cannot register user {Email}.", model.Email);
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.RegistrationFailed, result.Errors.Select(error => error.Description).ToList());
            }

            await userManager.AddToRoleAsync(user, AppRoles.User);

            IDictionary<string, object> claims = new Dictionary<string, object>
            {
                { "role", AppRoles.User }
            };

            logger.LogInformation("User {Email} created a new account with password.", user.Email);

            return await CreateAuthenticationResultAsync(user, claims);
        }

        /// <summary>
        /// генерация токена
        /// </summary>
        /// <param name="user">сущность пользователя</param>
        /// <param name="claims">клеймы пользоватляеля</param>
        /// <returns></returns>
        async Task<AuthenticationResponse> CreateAuthenticationResultAsync(User user, IDictionary<string, object> claims)
        {
            var settings = jwtSettings.Value;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(settings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.Add(jwtSettings.Value.TokenLifetime),
                Issuer = settings.Issuer,
                Claims =  claims,
                Audience = settings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken()
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.Add(jwtSettings.Value.RefreshTokenLifetime),
                User = user
            };

            var refreshTokenResponse = mapper.Map<GetRefreshToken>(refreshToken);
            refreshTokenResponse.User.Role = claims["role"].ToString() ?? AppRoles.User;

            await repository.UserRepository.AddRefreshTokenAsync(refreshToken);

            await repository.SaveAsync();

            var jwt = tokenHandler.WriteToken(token);

            return AuthenticationResponse.CreateSuccessfulResponse(jwt, refreshTokenResponse);
        }

        public async Task<AuthenticationResponse> LogOut(string token)
        {
            var storedRefreshToken = await repository.UserRepository.ReadRefreshTokenAsync(token);

            if (storedRefreshToken == null)
            {
                return AuthenticationResponse.CreateSuccessfulResponse();
            }

            repository.UserRepository.DeleteRefreshTokenAsync(storedRefreshToken);

            await repository.SaveAsync();

            return AuthenticationResponse.CreateSuccessfulResponse();
        }


        /// <summary>
        /// обновление токена
        /// </summary>
        /// <param name="model">модель обновления токена</param>
        public async Task<AuthenticationResponse> RefreshTokenAsync(string token)
        {
            var storedRefreshToken = await repository.UserRepository.ReadRefreshTokenAsync(token);

            if (storedRefreshToken == null)
            {
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.RefreshingTokenFailed, new List<string> { "refresh token not found" });
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.RefreshingTokenFailed, new List<string> { "refresh token expired" });
            }

            var user = await repository.UserRepository.ReadWithRoleAsync(storedRefreshToken.UserId);

            if (user == null)
            {
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.RefreshingTokenFailed, new List<string> { "user with refresh token doesnt exists" });
            }

            IDictionary<string, object> claims = new Dictionary<string, object>
            {
                { "role", user.Role }
            };

            return await CreateAuthenticationResultAsync(user.User, claims);
        }
        public async Task<AuthenticationResponse> CheckRefreshTokenExists(string refreshToken)
        {
            var storedRefreshToken = await repository.UserRepository.ReadRefreshTokenAsync(refreshToken);

            if (storedRefreshToken == null)
            {
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.RefreshTokenNotFound, new List<string> { "refresh token not found" });
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.RefreshingTokenFailed, new List<string> { "refresh token expired" });
            }

            var user = await repository.UserRepository.ReadWithRoleAsync(storedRefreshToken.UserId);
            var refreshTokenResponse = mapper.Map<GetRefreshToken>(storedRefreshToken);
            refreshTokenResponse.User.Role = user.Role;

            return AuthenticationResponse.CreateSuccessfulResponse(string.Empty, refreshTokenResponse);
        }

        string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

    }
}
