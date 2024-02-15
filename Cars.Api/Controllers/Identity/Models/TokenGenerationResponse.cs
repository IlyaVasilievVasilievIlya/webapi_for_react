namespace Cars.Api.Controllers.Identity.Models
{
    /// <summary>
    /// модель полученного ответа с токеном
    /// </summary>
    public class TokenGenerationResponse
    {
        /// <summary>
        /// токен доступа
        /// </summary>
        public required string AccessToken { get; set; }

        /// <summary>
        /// информация о пользователе
        /// </summary>
        public required UserInfoResponse UserInfo { get; set; }
    }
}
