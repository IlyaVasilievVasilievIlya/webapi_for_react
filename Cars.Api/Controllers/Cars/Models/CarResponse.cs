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
        public required string Brand { get; set; }

        /// <summary>
        /// модель
        /// </summary>
        public required string Model { get; set; }

        /// <summary>
        /// цвет
        /// </summary>
        public string? Color { get; set; }
    }
}
