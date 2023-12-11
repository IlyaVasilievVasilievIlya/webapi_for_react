using Microsoft.AspNetCore.Mvc;

namespace Cars.Api.Exceptions
{
    public class ProcessProblemDetails : ProblemDetails
    {
        public List<string>? Errors { get; set; }
    }
}
