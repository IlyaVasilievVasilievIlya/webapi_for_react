using AutoMapper;
using Cars.Api.Controllers.Users.Models;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cars.Api.Controllers.Users
{
    /// <summary>
    /// контроллер работы с пользователями
    /// </summary>
    [ApiController]
    [Authorize(Policy = AppPolicies.ViewUsers)]
    [Route("api/[controller]")]

    public class UsersController : ControllerBase
    {
        readonly IMapper mapper;
        readonly IUserService service;

        /// <summary>
        /// конструктор контроллера
        /// </summary>
        /// <param name="service"></param>
        /// <param name="mapper"></param>
        public UsersController(IUserService service, IMapper mapper)
        {
            this.mapper = mapper;
            this.service = service;
        }

        /// <summary>
        /// получение пользователей
        /// </summary>
        /// <param name="offset">смещение</param>
        /// <param name="limit">макс. значение</param>
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), 200)]
        public async Task<IEnumerable<UserResponse>> GetUsers(int offset = 0, int limit = 10)
        {
            var users = await service.GetUsersAsync(offset, limit);
            var response = mapper.Map<IEnumerable<UserResponse>>(users);

            return response;
        }

        /// <summary>
        /// изменение пользователя
        /// </summary>
        /// <param name="request">модель запроса на изменение</param>
        /// <param name="id">id пользователя</param>
        [HttpPut("{id}")]
        [Authorize(Policy = AppPolicies.EditUsers)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        [HttpPatch("{id}/role")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
