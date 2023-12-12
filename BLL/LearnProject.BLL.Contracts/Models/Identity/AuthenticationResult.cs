namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// класс ответов от сервиса аутентификации
    /// </summary>
    public class AuthenticationResult
    {
        private AuthenticationResult(bool result) : this(result, null, null) { }

        private AuthenticationResult(bool result, List<string>? errors) : this(result, errors, null, null) { }

        private AuthenticationResult(bool result, string? token, string? refreshToken) : this(result, null, token, refreshToken) { }

        private AuthenticationResult(bool result, List<string>? errors, string? token, string? refreshToken)
        {
            IsSuccessful = result;
            Errors = errors;
            Token = token;
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// успешность операции
        /// </summary>
        public bool IsSuccessful { get; private set; }

        /// <summary>
        /// ошибка
        /// </summary>
        public List<string>? Errors { get; private set; }

        /// <summary>
        /// токен доступа
        /// </summary>
        public string? Token { get; private set; }

        /// <summary>
        /// refresh токен
        /// </summary>
        public string? RefreshToken {  get; private set; }


        /// <summary>
        /// создать модель с успешным результатом
        /// </summary>
        /// <returns>созданная модель</returns>
        public static AuthenticationResult CreateSuccessfulResponse(string token, string refreshToken)
        {
            return new AuthenticationResult(true, token, refreshToken);
        }

        /// <summary>
        /// создать модель с успешным результатом
        /// </summary>
        /// <returns>созданная модель</returns>
        public static AuthenticationResult CreateSuccessfulResponse()
        {
            return new AuthenticationResult(true);
        }

        /// <summary>
        /// создать модель с ошибкой
        /// </summary>
        /// <returns>созданная модель</returns>
        public static AuthenticationResult CreateFailedResponse(List<string> errors)
        {
            return new AuthenticationResult(false, errors);
        }
    }
}
