namespace LearnProject.BLL.Contracts.Models
{
    /// <summary>
    /// модель принимаемая сервисом для изменения пользователя
    /// </summary>
    public class UpdateUserModel
    {
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
