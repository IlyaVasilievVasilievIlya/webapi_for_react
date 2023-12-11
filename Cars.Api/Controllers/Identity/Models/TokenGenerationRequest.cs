using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Identity.Models
{
    public class TokenGenerationRequest
    {
        [Required]
        [EmailAddress]
        public required string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string Password { get; set; }
    }
}
