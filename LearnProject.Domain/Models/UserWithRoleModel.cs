using LearnProject.Domain.Entities;

namespace LearnProject.Domain.Models
{
    /// <summary>
    /// модель сущности пользователя с его ролью, отдаваемая репозиторием
    /// </summary>
    public class UserWithRoleModel
    {
        /// <summary>
        /// сущность User
        /// </summary>
        public required User User { get; set; }

        /// <summary>
        /// роль пользователя
        /// </summary>
        public required string Role { get; set; }
    }
}
