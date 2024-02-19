using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Cars.Models
{
    /// <summary>
    /// модель запроса на добавление машины
    /// </summary>
    public class AddCarRequest
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
