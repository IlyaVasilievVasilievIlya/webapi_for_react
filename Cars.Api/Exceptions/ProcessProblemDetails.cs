using Microsoft.AspNetCore.Mvc;

namespace Cars.Api.Exceptions
{
    /// <summary>
    /// обертка над ProblemDetails со списком ошибок
    /// </summary>
    public class ProcessProblemDetails : ProblemDetails
    {
        /// <summary>
        /// ошибки
        /// </summary>
        public List<string>? Errors { get; set; }
    }
}
