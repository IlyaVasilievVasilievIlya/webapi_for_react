using Cars.Api.Controllers.Identity.Models;
using System.ComponentModel.DataAnnotations;

namespace Cars.Api.Controllers.Users.Models
{
    /// <summary>
    /// модель запроса на редактирование
    /// </summary>
    public class UpdateUserRequest
    {
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
}
