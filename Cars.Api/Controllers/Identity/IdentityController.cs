using AutoMapper;
using Cars.Api.Controllers.Identity.Models;
using Google.Apis.Auth;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Api.Controllers.Identity
{
    /// <summary>
    /// контроллер для работы с токенами
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        readonly IIdentityService identityService;

        readonly IMapper mapper;

        /// <summary>
        /// конструктор контроллера
        /// </summary>
        /// <param name="identityService">сервис аутентификации</param>
        /// <param name="mapper">маппер</param>
        public IdentityController(IIdentityService identityService, IMapper mapper)
        {
            this.identityService = identityService;
            this.mapper=mapper; 
        }

        /// <summary>
        /// регистрация
        /// </summary>
        /// <param name="request">модель запроса на регистрацию</param>
        [ProducesResponseType(typeof(TokenGenerationResponse), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPost("signup")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var model = mapper.Map<RegisterUserModel>(request);
            var authResponse = await identityService.RegisterAsync(model);

            if(authResponse.Result == AuthenticationResult.RegistrationFailed)
            {
                return Unauthorized(authResponse.Errors);
            }

            if (authResponse.Result == AuthenticationResult.UserAlreadyExists)
            {
                return BadRequest(authResponse.Errors);
            }

            SetRefreshToken(authResponse.RefreshToken!);

            return Ok(new TokenGenerationResponse()
            {
                AccessToken = authResponse.Token!,
                UserInfo = mapper.Map<UserInfoResponse>(authResponse.RefreshToken!.User)
            });
        }

        /// <summary>
        /// создание токенов
        /// </summary>
        /// <param name="request">модель запроса для входа</param>
        [HttpPost("login")]
        [ProducesResponseType(typeof(TokenGenerationResponse), 200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var model = mapper.Map<LoginUserModel>(request);
            var authResponse = await identityService.LogInAsync(model);

            if (authResponse.Result == AuthenticationResult.UserDoesNotExist)
            {
                return Unauthorized(authResponse.Errors);
            }

            if (authResponse.Result == AuthenticationResult.InvalidPasswordWhileLogin)
            {
                return Unauthorized(authResponse.Errors);
            }

            SetRefreshToken(authResponse.RefreshToken!);

            return Ok(new TokenGenerationResponse()
            {
                AccessToken = authResponse.Token!,
                UserInfo = mapper.Map<UserInfoResponse>(authResponse.RefreshToken!.User)
            });
        }



        /// <summary>
        /// вход через Google
        /// </summary>
        /// <param name="token">токен отправленный клиентом</param>
        [HttpPost("loginWithGoogle")]
        [ProducesResponseType(typeof(TokenGenerationResponse), 200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string token)
        {
            var authResponse = await identityService.LogInWithGoogleAsync(token);

            if (authResponse.Result == AuthenticationResult.TokenValidationFailed)
            {
                return Unauthorized(authResponse.Errors);
            }

            if (authResponse.Result == AuthenticationResult.RegistrationFailed)
            {
                return Unauthorized(authResponse.Errors);
            }

            SetRefreshToken(authResponse.RefreshToken!);

            return Ok(new TokenGenerationResponse()
            {
                AccessToken = authResponse.Token!,
                UserInfo = mapper.Map<UserInfoResponse>(authResponse.RefreshToken!.User)
            });
        }

        /// <summary>
        /// перевыпуск токена
        /// </summary>
        /// <param name="request">модель запроса для перевыпуска токенов</param>
        [HttpPost("token/refreshing")]
        [ProducesResponseType(typeof(TokenGenerationResponse), 200)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (refreshToken == null)
            {
                return Unauthorized(new List<string> { "Refresh token not found" });
            }

            var authResponse = await identityService.RefreshTokenAsync(refreshToken);

            if (authResponse.Result == AuthenticationResult.RefreshingTokenFailed)
            {
                return Unauthorized(authResponse.Errors);
            }

            SetRefreshToken(authResponse.RefreshToken!);

            return Ok(new TokenGenerationResponse()
            {
                AccessToken = authResponse.Token!,
                UserInfo = mapper.Map<UserInfoResponse>(authResponse.RefreshToken!.User)
            });
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> LogOut()
        {
            var refreshToken = Request.Cookies["RefreshToken"];

            if (refreshToken == null)
            {
                return NoContent();
            }

            await identityService.LogOut(refreshToken);

            HttpContext.Response.Cookies.Delete("RefreshToken");

            return NoContent();
        }

        void SetRefreshToken(GetRefreshToken token)
        {
            HttpContext.Response.Cookies.Append("RefreshToken", token.Token, new CookieOptions
            {
                Expires = token.ExpiryDate,
                HttpOnly = true
            });
        }
    }
}
