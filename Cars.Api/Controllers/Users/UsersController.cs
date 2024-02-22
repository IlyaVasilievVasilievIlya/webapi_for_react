using AutoMapper;
using Cars.Api.Controllers.Users.Models;
using LearnProject.BLL.Contracts;
using LearnProject.BLL.Contracts.Models;
using LearnProject.Domain.Entities;
using LearnProject.Domain.Models;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

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
        /// <param name="parameters">параметры запроса</param>
        [HttpGet("")]
        [ProducesResponseType(typeof(IEnumerable<UserResponse>), 200)]
        public IEnumerable<UserResponse> GetUsers([FromQuery] UserQueryParameters parameters)
        {
            var users = service.GetUsers(parameters);
            var response = mapper.Map<IEnumerable<UserResponse>>(users);

            var metadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages
            };

            Response.Headers.Append("Pagination", JsonSerializer.Serialize(metadata));

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

            return NoContent();
        }

        /// <summary>
        /// смена роли пользователя
        /// </summary>
        /// <param name="id">id пользователя</param>
        /// <param name="newRole">название новой роли</param>
        [HttpPatch("{id}/role")]
        [Authorize(Policy = AppPolicies.EditRoles)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUserRole(string id, [FromBody] string newRole)
        {
            ServiceResponse<int> response = await service.ChangeRoleAsync(id, newRole);

            if (!response.IsSuccessful)
            {
                return BadRequest(response.Error);
            }

            return NoContent();
        }
    }
}
