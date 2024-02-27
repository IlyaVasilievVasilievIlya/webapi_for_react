namespace Cars.Api.Controllers.Identity.Models
{
    /// <summary>
    /// информация о пользователе после успешного входа / регистрации
    /// </summary>
    public class UserInfoResponse
    {
        public required string Role { get; set; }

        public required bool EmailConfirmed { get; set; }
    }
}
