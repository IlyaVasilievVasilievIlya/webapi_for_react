namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель токена для обновления
    /// </summary>
    public class RefreshTokenUserModel
    {
        /// <summary>
        /// refresh токен
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
