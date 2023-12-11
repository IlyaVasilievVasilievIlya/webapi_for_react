using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Users.Models
{
    /// <summary>
    /// модель запроса для регистрации
    /// </summary>
    public class RegisterRequest
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public required string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        /// <summary>
        /// имя
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "Length less than 100 characters")]
        public required string Name { get; set; }

        /// <summary>
        /// фамилия
        /// </summary>
        [Required]
        [MaxLength(100, ErrorMessage = "Length less than 100 characters")]
        public required string Surname { get; set; }

        /// <summary>
        /// отчество
        /// </summary>
        [MaxLength(100, ErrorMessage = "Length less than 100 characters")]
        public string? Patronymic { get; set; }

        /// <summary>
        /// дата рождения
        /// </summary>
        [DataType(DataType.Date)]
        [DateRange("1.1.1923")]
        public DateOnly BirthDate { get; set; }
    }

    public class DateRange : ValidationAttribute
    {
        DateOnly MinDate;

        public DateRange(string minDate)
        {
            MinDate = DateOnly.Parse(minDate);
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var d = (DateOnly)value!;
            if (d < MinDate || d > DateOnly.FromDateTime(DateTime.Now))
                return new ValidationResult(ErrorMessage);
            else
                return ValidationResult.Success;
        }
    }
}
