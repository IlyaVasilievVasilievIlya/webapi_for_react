using AutoMapper;
using Google.Apis.Auth;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Entities;
using LearnProject.Domain.Repositories;
using LearnProject.Shared.Common;
using LearnProject.Shared.Common.Settings;
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
        readonly JwtSettings jwtSettings;
        readonly ExternalProviders providers;
        readonly UserManager<User> userManager;

        public IdentityService(IRepositoryWrapper repository, IMapper mapper, ILogger<IdentityService> logger,
            JwtSettings jwtSettings, ExternalProviders providers, UserManager<User> userManager)
        {
            this.repository = repository;
            this.mapper = mapper;
            this.logger = logger;
            this.jwtSettings = jwtSettings;
            this.providers = providers;
            this.userManager = userManager;
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

            return await CreateAuthenticationResultForExistingUser(existingUser);
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
                logger.LogInformation(e.Message);
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.TokenValidationFailed,
                    new List<string> { "Google authentication failed - invalidToken" });
            }

            var existingUser = await userManager.FindByEmailAsync(payload.Email);

            if (existingUser != null) 
            {
                return await CreateAuthenticationResultForExistingUser(existingUser);
            }

            User newUser = new User()
            {
                Name = payload.GivenName,
                Surname = payload.FamilyName,
                Email = payload.Email
            };

            return await CreateAuthenticationResultForNewUser(newUser);
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

            return await CreateAuthenticationResultForNewUser(user, model.Password);
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

            logger.LogInformation("User {Email} logged out.", storedRefreshToken.User.Email);

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

        private async Task<AuthenticationResponse> CreateAuthenticationResultForNewUser(User newUser, string? password = null)
        {
            var result = password == null ? await userManager.CreateAsync(newUser) : await userManager.CreateAsync(newUser, password);

            if (!result.Succeeded)
            {
                logger.LogInformation("Cannot register user {Email}.", newUser.Email);
                return AuthenticationResponse.CreateFailedResponse(AuthenticationResult.RegistrationFailed, result.Errors.Select(error => error.Description).ToList());
            }

            await userManager.AddToRoleAsync(newUser, AppRoles.User);

            IDictionary<string, object> newUserClaims = new Dictionary<string, object>
            {
                { "role", AppRoles.User }
            };

            logger.LogInformation("User {Email} created.", newUser.Email);

            return await CreateAuthenticationResultAsync(newUser, newUserClaims);
        }

        Task<AuthenticationResponse> CreateAuthenticationResultForExistingUser(User user)
        {
            var role = repository.UserRepository.GetUserRole(user.Id);

            IDictionary<string, object> claims = new Dictionary<string, object>
            {
                { "role", role }
            };

            logger.LogInformation("User {Email} logged in", user.Email);

            return CreateAuthenticationResultAsync(user, claims);
        }

        /// <summary>
        /// генерация токена
        /// </summary>
        /// <param name="user">сущность пользователя</param>
        /// <param name="claims">клеймы пользоватляеля</param>
        /// <returns></returns>
        async Task<AuthenticationResponse> CreateAuthenticationResultAsync(User user, IDictionary<string, object> claims)
        {
            var jwt = GenerateToken(claims);

            var refreshTokenResponse = await CreateRefreshToken(user, claims);

            return AuthenticationResponse.CreateSuccessfulResponse(jwt, refreshTokenResponse);
        }

        string GenerateToken(IDictionary<string, object> claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.Add(jwtSettings.TokenLifetime),
                Issuer = jwtSettings.Issuer,
                Claims = claims,
                Audience = jwtSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        async Task<GetRefreshToken> CreateRefreshToken(User user, IDictionary<string, object> claims)
        {
            var refreshToken = new RefreshToken()
            {
                Token = GenerateRefreshToken(),
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.Add(jwtSettings.RefreshTokenLifetime),
                User = user
            };

            var refreshTokenResponse = mapper.Map<GetRefreshToken>(refreshToken);
            refreshTokenResponse.User.Role = claims["role"].ToString() ?? AppRoles.User;

            await repository.UserRepository.AddRefreshTokenAsync(refreshToken);

            await repository.SaveAsync();

            return refreshTokenResponse;
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
