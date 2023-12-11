namespace Cars.Api.Exceptions
{
    public class ProcessException : Exception
    {
        public ProcessProblemDetails? Details { get; set; }

        public ProcessException(ProcessProblemDetails details)
        {
            Details = details;
        }
    }
}
