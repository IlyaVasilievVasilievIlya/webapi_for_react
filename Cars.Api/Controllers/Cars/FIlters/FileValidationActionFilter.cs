using Cars.Api.Controllers.Cars.Models;
using Cars.Api.Exceptions;
using LearnProject.Shared.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cars.Api.Controllers.Cars.FIlters
{
    public class FileValidationActionFilter : Attribute, IAsyncActionFilter
    {
        readonly FileUploadSettings settings;

        public FileValidationActionFilter(FileUploadSettings settings)
        {
            this.settings = settings;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.ActionArguments.TryGetValue("request", out object? model);
            if (model == null) 
            {
               await next();
               return;
            }
            var filePropInfo = model.GetType().GetProperties().Where(value => value.PropertyType == typeof(IFormFile)).FirstOrDefault();
            if ( filePropInfo == null)
            {
                await next();
                return;
            }
            var file = filePropInfo.GetValue(model);
            if (file != null && !(checkFileSize(file as IFormFile) && checkFileExtension(file as IFormFile)))
            {
                context.Result = new ObjectResult(context.ModelState)
                {
                    Value = new ProcessProblemDetails() { Title = "file is invalid" },
                    StatusCode = StatusCodes.Status400BadRequest
                };
                return;
            }
            await next();
        }

        bool checkFileExtension(IFormFile file)
        {
            var extensions = settings.AllowedExtensions.Split(";");
            return extensions.Contains(file.ContentType);
        }

        bool checkFileSize(IFormFile file)
        {
            return file.Length > 0 && file.Length <= int.Parse(settings.MAX_SIZE);
        }
    }
}
