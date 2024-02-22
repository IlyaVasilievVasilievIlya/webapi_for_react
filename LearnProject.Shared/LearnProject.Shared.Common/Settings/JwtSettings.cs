namespace LearnProject.Shared.Common.Settings
{
    /// <summary>
    /// настройки jwt для доступа к api
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// издатель
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// получатель
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// секретный ключ
        /// </summary>
        public string Key { get; set; } = string.Empty;

        /// <summary>
        /// время жизни токена доступа
        /// </summary>
        public TimeSpan TokenLifetime { get; set; } = new TimeSpan(0, 5, 0);

        /// <summary>
        /// время жизни токена refresh
        /// </summary>
        public TimeSpan RefreshTokenLifetime { get; set; } = new TimeSpan(20, 0, 0);
    }
}
