using LearnProject.Domain.Entities;

namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель токена для обновления
    /// </summary>
    public class GetRefreshToken
    {
        /// <summary>
        /// токен
        /// </summary>
        public required string Token { get; set; }

        /// <summary>
        /// дата создания
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// дата истечения срока
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// навигационное свойство
        /// </summary>
        public GetUserModel User { get; set; }
    }
}
