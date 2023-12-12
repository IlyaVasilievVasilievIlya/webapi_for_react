namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель запроса входа для userService
    /// </summary>
    public class LoginUserModel
    {
        /// <summary>
        /// адрес почты
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// пароль
        /// </summary>
        public required string Password { get; set; }
    }
}
