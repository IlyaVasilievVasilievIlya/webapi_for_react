using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Identity.Models
{
    /// <summary>
    /// модель запроса для входа
    /// </summary>
    public class LoginRequest
    {
        /// <summary>
        /// логин
        /// </summary>
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// пароль
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
