namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель токена для обновления
    /// </summary>
    public class RefreshTokenUserModel
    {
        /// <summary>
        /// токен доступа
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// refresh токен
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
