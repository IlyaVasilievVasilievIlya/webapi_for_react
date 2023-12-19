namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель запроса добавления для UserService
    /// </summary>
    public class RegisterUserModel
    {
        /// <summary>
        /// адрес почты
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// пароль
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// имя
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// фамилия
        /// </summary>
        public required string Surname { get; set; }

        /// <summary>
        /// отчество
        /// </summary>
        public string? Patronymic { get; set; }

        /// <summary>
        /// дата рождения
        /// </summary>
        public DateOnly BirthDate { get; set; }
    }
}
