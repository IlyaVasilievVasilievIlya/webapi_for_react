namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель пользователя, выдаваемая сервисом при запросе на получение
    /// </summary>
    public class GetUserModel
    {
        /// <summary>
        /// id пользователя
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// адрес почты
        /// </summary>
        public string? Email { get; set; }

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

        /// <summary>
        /// роль
        /// </summary>
        public required string Role { get; set; }
    }
}
