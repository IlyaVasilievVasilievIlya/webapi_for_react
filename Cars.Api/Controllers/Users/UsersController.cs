using AutoMapper;
using Cars.Api.Controllers.Users.Models;
using Cars.Api.Exceptions;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Cars.Api.Controllers.Users
{
    [ApiController]
    [Authorize(Policy = AppPolicies.ViewUsers)]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        readonly IMapper mapper;
        readonly IUserService service;
        readonly ILogger logger;


        public UsersController(IUserService service, IMapper mapper, ILogger logger)
        {
            this.mapper = mapper;
            this.service = service;
            this.logger = logger;
        }

        /// <summary>
        /// получение пользователей
        /// </summary>
        [HttpGet("")]
        public async Task<IEnumerable<UserResponse>> GetUsers(int offset = 0, int limit = 10)
        {
            var users = await service.GetUsersAsync(offset, limit);
            var response = mapper.Map<IEnumerable<UserResponse>>(users);

            return response;
        }

        /// <summary>
        /// регистрация
        /// </summary>
        [HttpPost("")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var model = mapper.Map<AddUserModel>(request);
            model.Role = AppRoles.User.ToUpper();

            ServiceResponse<IdentityResult> response = await service.Register(model);

            if (!response.IsSuccessful)
            {
                logger.LogInformation("Cannot register user {Email}.", model.Email);
                throw new ProcessException(new ProcessProblemDetails() { Errors = response?.Value?.Errors.Select(er => er.Description).ToList()});
            }

            logger.LogInformation("User {Email} created a new account with password.", model.Email);

            return Ok();
        }

        ///// <summary>
        ///// регистрация
        ///// </summary>
        //[HttpPost("")]
        //[AllowAnonymous]
        //public async Task<IActionResult> LogIn(LoginRequest request)
        //{
        //    LoginUserModel model = mapper.Map<LoginUserModel>(request);

        //    ServiceResponse<SignInResult> response = await service.LogIn(model);

        //    if (!response.IsSuccessful)
        //    {
        //        logger.LogInformation("Invalid login attempt.");
        //        throw new ProcessException(new ProcessProblemDetails() { Errors = { response?.Value?.ToString() ?? string.Empty  } }); //TODO 
        //    }

        //    logger.LogInformation("User {Email} logged in.", model.Email);

        //    return Ok();
        //}

        /// <summary>
        /// изменение пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        [HttpPut("{id}")]
        [Authorize(Policy = AppPolicies.EditUsers)]
        public async Task<IActionResult> UpdateUser(string id, UpdateUserRequest request)
        {
            var model = mapper.Map<UpdateUserModel>(request);

            ServiceResponse<int> response = await service.UpdateUserAsync(id, model);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Error);
            }

            return Ok();
        }

        /// <summary>
        /// смена роли пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="newRole">название новой роли</param>
        [Authorize(Policy = AppPolicies.EditRoles)]
        public async Task<IActionResult> ChangeUserRole(string id, string newRole)
        {
            ServiceResponse<int> response = await service.ChangeRoleAsync(id, newRole);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Error);
            }

            return Ok();
        }
    }
}
