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
        /// refresh токен
        /// </summary>
        public required string RefreshToken { get; set; }
    }
}
