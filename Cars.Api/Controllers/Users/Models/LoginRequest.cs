using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Users.Models
{
    /// <summary>
    /// модель запроса для входа
    /// </summary>
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
