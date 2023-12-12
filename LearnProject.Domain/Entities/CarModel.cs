namespace LearnProject.Domain.Entities
{
    /// <summary>
    /// модель модели машины
    /// </summary>
    public class CarModel
    {
        /// <summary>
        /// id
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// марка авто
        /// </summary>
        public required string Brand { get; set; }

        /// <summary>
        /// модель авто
        /// </summary>
        public required string Name { get; set; }
    }
}
