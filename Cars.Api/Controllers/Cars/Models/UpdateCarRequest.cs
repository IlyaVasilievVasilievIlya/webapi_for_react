using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Cars.Models
{
    /// <summary>
    /// запрос на редактирование авто
    /// </summary>
    public class UpdateCarRequest
    {
        [Required]
        /// <summary>
        /// id модели авто
        /// </summary>
        public int CarModelId { get; set; }

        [MaxLength(100, ErrorMessage = "Length less than 100 characters")]
        /// <summary>
        /// цвет машины
        /// </summary>
        public string? Color { get; set; }

        [Required]
        public IFormFile Image { get; set; }
    }
}
