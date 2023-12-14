using AutoMapper;
using Cars.Api.Controllers.Identity.Models;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
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

            return Ok(new TokenGenerationResponse()
            {
                AccessToken = authResponse.Token!,
                RefreshToken = authResponse.RefreshToken!
            });
        }

        /// <summary>
        /// создание токенов
        /// </summary>
        /// <param name="request">модель запроса для входа</param>
        [HttpPost("login")]
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

            return Ok(new TokenGenerationResponse()
            {
                AccessToken = authResponse.Token!,
                RefreshToken = authResponse.RefreshToken!
            });
        }

        /// <summary>
        /// перевыпуск токена
        /// </summary>
        /// <param name="request">модель запроса для перевыпуска токенов</param>
        [HttpPost("token/refreshing")]
        public async Task<IActionResult> RefreshToken(TokenRefreshRequest request)
        {
            var model = mapper.Map<RefreshTokenUserModel>(request);
            var authResponse = await identityService.RefreshTokenAsync(model);

            if (authResponse.Result == AuthenticationResult.RefreshingTokenFailed)
            {
                return Unauthorized(authResponse.Errors);
            }

            return Ok(new TokenGenerationResponse()
            {
                AccessToken = authResponse.Token!,
                RefreshToken = authResponse.RefreshToken!
            });
        }
    }
}
