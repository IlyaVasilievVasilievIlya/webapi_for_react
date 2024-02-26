using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Identity.Models
{
    public class ResetPasswordRequest
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
