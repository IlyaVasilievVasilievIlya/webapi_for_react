namespace LearnProject.BLL.Contracts.Models
{
    public enum AuthenticationResult
    {
        Successed,
        UserAlreadyExists,
        RegistrationFailed,
        UserDoesNotExist,
        InvalidPasswordWhileLogin,
        RefreshingTokenFailed
    }

    /// <summary>
    /// класс ответов от сервиса аутентификации
    /// </summary>
    public class AuthenticationResponse
    {
        private AuthenticationResponse(AuthenticationResult result) : this(result, null, null) { }

        private AuthenticationResponse(AuthenticationResult result, List<string>? errors) : this(result, errors, null, null) { }

        private AuthenticationResponse(AuthenticationResult result, string? token, string? refreshToken) : this(result, null, token, refreshToken) { }

        private AuthenticationResponse(AuthenticationResult result, List<string>? errors, string? token, string? refreshToken)
        {
            Result = result;
            Errors = errors;
            Token = token;
            RefreshToken = refreshToken;
        }

        /// <summary>
        /// результат ответа
        /// </summary>
        public AuthenticationResult Result {  get; private set; }

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
        public static AuthenticationResponse CreateSuccessfulResponse(string token, string refreshToken)
        {
            return new AuthenticationResponse(AuthenticationResult.Successed, token, refreshToken);
        }

        /// <summary>
        /// создать модель с успешным результатом
        /// </summary>
        /// <returns>созданная модель</returns>
        public static AuthenticationResponse CreateSuccessfulResponse()
        {
            return new AuthenticationResponse(AuthenticationResult.Successed);
        }

        /// <summary>
        /// создать модель с ошибкой
        /// </summary>
        /// <returns>созданная модель</returns>
        public static AuthenticationResponse CreateFailedResponse(AuthenticationResult result, List<string> errors)
        {
            return new AuthenticationResponse(result, errors);
        }
    }
}
