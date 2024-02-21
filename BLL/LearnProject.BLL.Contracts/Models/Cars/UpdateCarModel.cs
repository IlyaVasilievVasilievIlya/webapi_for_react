namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель редактирования машины
    /// </summary>
    public class UpdateCarModel
    {
        /// <summary>
        /// id авто
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// id модели авто
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// цвет машины
        /// </summary>
        public string? Color { get; set; }

        public Stream? Image { get; set; }
    }
}
