using Microsoft.AspNetCore.Identity;

namespace LearnProject.Domain.Entities
{
    /// <summary>
    /// модель пользователя
    /// </summary>
    public class User : IdentityUser
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
