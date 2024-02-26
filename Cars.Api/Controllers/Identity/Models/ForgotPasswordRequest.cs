using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Identity.Models
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
