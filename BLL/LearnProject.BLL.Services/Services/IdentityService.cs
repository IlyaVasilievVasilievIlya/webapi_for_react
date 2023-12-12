using AutoMapper;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Entities;
using LearnProject.Domain.Repositories;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LearnProject.BLL.Services.Services
{
    public class IdentityService : IIdentityService
    {
        readonly IUserRepository userRepository;
        readonly IMapper mapper;
        readonly ILogger<IdentityService> logger;
        readonly IOptions<JwtSettings> jwtSettings;
        readonly TokenValidationParameters tokenParameters;
        readonly UserManager<User> userManager;

        public IdentityService(IUserRepository userRepository, IMapper mapper, ILogger<IdentityService> logger,
            IOptions<JwtSettings> jwtSetttings, TokenValidationParameters tokenParameters,
            UserManager<User> userManager)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
            this.logger = logger;
            this.jwtSettings = jwtSetttings;
            this.tokenParameters = tokenParameters;
            this.userManager = userManager;
        }

        /// <summary>
        /// вход в приложение
        /// </summary>
        /// <param name="model">модель данных для входа</param>
        /// <returns>ответ с моделью AuthenticationResult</returns>
        public async Task<AuthenticationResult> LogInAsync(LoginUserModel model)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);

            if (existingUser == null)
            {
                logger.LogInformation("Cannot login user {Email}.", model.Email);
                return AuthenticationResult.CreateFailedResponse(new List<string> { "user doesnt exist" });
            }

            var checkPassword = await userManager.CheckPasswordAsync(existingUser, model.Password);

            if (!checkPassword)
            {
                logger.LogInformation("Cannot login user {Email}.", model.Email);
                return AuthenticationResult.CreateFailedResponse(new List<string> { "invalid password" });
            }

            var role = (await userRepository.ReadWithRoleAsync(existingUser.Id)).Role;

            IDictionary<string, object> claims = new Dictionary<string, object>
            {
                { "role", role }
            };

            return await CreateAuthenticationResultAsync(existingUser, claims);
        }

        /// <summary>
        /// регистрация пользователя
        /// </summary>
        /// <param name="model">модель добавления пользователя</param>
        /// <returns>ответ с моделью AuthenticationResult</returns>
        public async Task<AuthenticationResult> RegisterAsync(RegisterUserModel model)
        {
            var existingUser = await userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                logger.LogInformation("Cannot register user {Email}.", model.Email);
                return AuthenticationResult.CreateFailedResponse(new List<string> { "user already exists" });
            }

            User user = mapper.Map<User>(model);

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                logger.LogInformation("Cannot register user {Email}.", model.Email);
                return AuthenticationResult.CreateFailedResponse(result.Errors.Select(error => error.Description).ToList());
            }

            await userManager.AddToRoleAsync(user, AppRoles.User);

            IDictionary<string, object> claims = new Dictionary<string, object>
            {
                { "role", AppRoles.User }
            };

            return await CreateAuthenticationResultAsync(user, claims);
        }

        /// <summary>
        /// генерация токена
        /// </summary>
        /// <param name="user">сущность пользователя</param>
        /// <param name="claims">клеймы пользоватляеля</param>
        /// <returns></returns>
        async Task<AuthenticationResult> CreateAuthenticationResultAsync(User user, IDictionary<string, object> claims)
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
                ExpiryDate = DateTime.UtcNow.Add(jwtSettings.Value.RefreshTokenLifetime)
            };

            await userRepository.AddRefreshTokenAsync(refreshToken);

            var jwt = tokenHandler.WriteToken(token);

            logger.LogInformation("User {Email} created a new account with password.", user.Email);
            return AuthenticationResult.CreateSuccessfulResponse(jwt, refreshToken.Token!);
        }

        /// <summary>
        /// провалидировать токен
        /// </summary>
        /// <param name="token">токен доступа</param>
        /// <returns>ClaimsPrincipal объект</returns>
        public ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal claimsPrincipal;
            try
            {
                tokenParameters.ValidateLifetime = false;
                claimsPrincipal = tokenHandler.ValidateToken(token, tokenParameters, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                tokenParameters.ValidateLifetime = true;
            }

            return claimsPrincipal;
        }

        /// <summary>
        /// обновление токена
        /// </summary>
        /// <param name="token">токен доступа</param>
        /// <param name="refreshToken">refresh токен</param>
        public async Task<AuthenticationResult> RefreshTokenAsync(RefreshTokenUserModel model)
        {
            var validatedToken = GetPrincipalFromToken(model.AccessToken);

            if (validatedToken == null)
            {
                return AuthenticationResult.CreateFailedResponse(new List<string> { "token is invalid" });
            }

            var storedRefreshToken = await userRepository.ReadRefreshTokenAsync(model.RefreshToken);


            if (storedRefreshToken == null)
            {
                return AuthenticationResult.CreateFailedResponse(new List<string> { "refresh token not found" });
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return AuthenticationResult.CreateFailedResponse(new List<string> { "refresh token expired" });
            }

            var user = await userRepository.ReadWithRoleAsync(storedRefreshToken.UserId);

            if (user == null)
            {
                return AuthenticationResult.CreateFailedResponse(new List<string> { "user with refresh token doesnt exists" });
            }

            IDictionary<string, object> claims = new Dictionary<string, object>
            {
                { "role", user.Role }
            };

            return await CreateAuthenticationResultAsync(user.User, claims);
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
