namespace LearnProject.Shared.Common
{
    /// <summary>
    /// настройки jwt для доступа к api
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// издатель
        /// </summary>
        public required string Issuer { get; set; }

        /// <summary>
        /// получатель
        /// </summary>
        public required string Audience { get; set; }

        /// <summary>
        /// секретный ключ
        /// </summary>
        public required string Key { get; set; }

        /// <summary>
        /// время жизни токена доступа
        /// </summary>
        public required TimeSpan TokenLifetime { get; set; }

        /// <summary>
        /// время жизни токена refresh
        /// </summary>
        public required TimeSpan RefreshTokenLifetime { get; set; }
    }
}
