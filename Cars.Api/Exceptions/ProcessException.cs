namespace Cars.Api.Exceptions
{
    /// <summary>
    /// тип ошибки для возврата стандартизованного ProblemDetails (возможно наследовать для уточнения ошибки)
    /// </summary>
    public class ProcessException : Exception
    {
        /// <summary>
        /// детали ошибки
        /// </summary>
        public ProcessProblemDetails? Details { get; set; }

        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="details"></param>
        public ProcessException(ProcessProblemDetails details)
        {
            Details = details;
        }
    }
}
