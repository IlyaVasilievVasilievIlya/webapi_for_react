namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель добавления авто
    /// </summary>
    public class AddCarModel
    {
        /// <summary>
        /// id модели авто
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// цвет машины
        /// </summary>
        public string? Color { get; set; }

        public required Stream Image { get; set; }
    }
}
