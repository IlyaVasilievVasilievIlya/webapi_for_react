using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Identity.Models
{
    public class ConfirmEmailRequest
    {
        [Required]
        public required string Token { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }

    }
}
