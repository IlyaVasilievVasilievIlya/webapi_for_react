using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Users.Models
{
    /// <summary>
    /// запроса на изменение роли
    /// </summary>
    public class ChangeRoleRequest
    {
        /// <summary>
        /// роль
        /// </summary>
        [Required]
        public required string Role { get; set; }
    }
}
