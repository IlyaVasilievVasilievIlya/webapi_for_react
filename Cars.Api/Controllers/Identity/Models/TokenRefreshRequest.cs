namespace Cars.Api.Controllers.Identity.Models
{
    /// <summary>
    /// модель для запроса перевыпуска токена
    /// </summary>
    public class TokenRefreshRequest
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
