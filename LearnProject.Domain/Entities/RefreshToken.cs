namespace LearnProject.Domain.Entities
{

    /// <summary>
    /// refresh токен
    /// </summary>
    public class RefreshToken
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
        /// id пользователя
        /// </summary>
        public required string UserId {  get; set; }

        /// <summary>
        /// навигационное свойство
        /// </summary>
        public User User { get; set; } = null!;
    }
}
