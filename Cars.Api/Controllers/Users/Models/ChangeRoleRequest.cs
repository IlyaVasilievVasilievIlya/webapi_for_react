using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Users.Models
{
    public class ChangeRoleRequest
    {
        /// <summary>
        /// роль
        /// </summary>
        [Required]
        public required string Role { get; set; }
    }
}
