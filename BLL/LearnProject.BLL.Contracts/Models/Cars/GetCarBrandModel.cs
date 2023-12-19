namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель выдаваемая при получении марки авто
    /// </summary>
    public class GetCarBrandModel
    {
        /// <summary>
        /// id модели
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// бренд и марка
        /// </summary>
        public required string FullName { get; set; }
    }
}
