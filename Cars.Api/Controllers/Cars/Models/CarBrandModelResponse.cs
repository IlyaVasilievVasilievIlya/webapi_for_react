namespace Cars.Api.Controllers.Cars.Models
{
    /// <summary>
    /// модель выдаваемая при получении марки авто
    /// </summary>
    public class CarBrandModelResponse
    {
        /// <summary>
        /// id модели
        /// </summary>
        public int CarModelId { get; set; }

        /// <summary>
        /// бренд 
        /// </summary>
        public required string Brand { get; set; }

        /// <summary>
        /// марка 
        /// </summary>
        public required string Model { get; set; }
    }
}