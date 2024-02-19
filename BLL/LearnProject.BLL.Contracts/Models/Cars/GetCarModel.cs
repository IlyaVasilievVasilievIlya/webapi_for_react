using LearnProject.Domain.Entities;

namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель авто, выдаваемая сервисом при запросе на получение
    /// </summary>
    public class GetCarModel
    {
        /// <summary>
        /// id авто
        /// </summary>
        public int CarId { get; set; }

        /// <summary>
        /// внешний ключ
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// навигационное свойство
        /// </summary>
        public required GetCarBrandModel Model { get; set; }

        /// <summary>
        /// цвет
        /// </summary>
        public string? Color { get; set; }

        public byte[]? Image { get; set; }
    }
}
