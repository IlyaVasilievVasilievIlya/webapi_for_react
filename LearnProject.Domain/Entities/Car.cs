namespace LearnProject.Domain.Entities
{
    /// <summary>
    /// модель авто
    /// </summary>
    public class Car
    {
        /// <summary>
        /// id
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// внешний ключ
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// навигационное свойство
        /// </summary>
        public CarModel? CarModel { get; set; }

        /// <summary>
        /// цвет машины
        /// </summary>
        public string? Color { get; set; }
    }
}
