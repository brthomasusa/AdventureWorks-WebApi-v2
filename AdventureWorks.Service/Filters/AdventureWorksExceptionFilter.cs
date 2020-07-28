using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.Exceptions;

namespace AdventureWorks.Service.Filters
{
    public class AdventureWorksExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnv;

        public AdventureWorksExceptionFilter(IHostingEnvironment env)
        {
            _hostingEnv = env;
        }

        public override void OnException(ExceptionContext context)
        {
            bool isDevelopment = _hostingEnv.IsDevelopment();
            var ex = context.Exception;
            string stackTrace = (isDevelopment) ? context.Exception.StackTrace : string.Empty;
            string message = ex.Message;
            string error = string.Empty;
            IActionResult actionResult;

            switch(ex)
            {
                case DbUpdateConcurrencyException ce:
                    // Returns a 400
                    error = "Concurrency Issue";
                    actionResult = new BadRequestObjectResult(new {error = error, message = message, StackTrace = stackTrace});
                    break;
                case AdventureWorksException awe:
                    // Return 400
                    error = "AdventureWorksException";
                    actionResult = new BadRequestObjectResult(new {error = error, message = message, StackTrace = stackTrace});
                    break;
                default:
                    error = "General Error";
                    actionResult = new ObjectResult(new {error = error, message = message, StackTrace = stackTrace})
                    {
                        StatusCode = 500
                    };
                    break;
            }

            context.Result = actionResult;
        }
    }
}