namespace Cars.Api.Controllers.Cars.Models
{
    /// <summary>
    /// ответ со списком машин
    /// </summary>
    public class CarResponse
    {
        /// <summary>
        /// id авто
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// марка
        /// </summary>
        public required CarBrandModelResponse Brand { get; set; }

        /// <summary>
        /// цвет
        /// </summary>
        public string? Color { get; set; }

        public byte[]? Image { get; set; }
    }
}
