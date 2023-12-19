namespace Cars.Api.Controllers.Users.Models
{
    /// <summary>
    /// модель ответа на запрос на получение пользователя
    /// </summary>
    public class UserResponse
    {
        /// <summary>
        /// id пользователя
        /// </summary>
        public required string Id { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public required string FullName { get; set; }

        /// <summary>
        /// почта (логин)
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public required string BirthDate { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public required string Role { get; set; }
    }
}
