using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.Exceptions;
using LoggerService;

namespace AdventureWorks.Service.Filters
{
    public class AdventureWorksExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnv;
        private ILoggerManager _logger;

        public AdventureWorksExceptionFilter(ILoggerManager logger, IHostingEnvironment env)
        {
            _logger = logger;
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

            switch (ex)
            {
                case DbUpdateConcurrencyException ce:
                    // Returns a 400
                    error = "Concurrency Issue";
                    actionResult = new BadRequestObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                case AdventureWorksUniqueIndexException une:
                    // Return 400
                    error = "Duplicate value detected!";
                    actionResult = new BadRequestObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                case AdventureWorksInvalidEntityIdException une:
                    // Return 400
                    error = "Can't find entity; invalid primary key value given.";
                    actionResult = new BadRequestObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                case AdventureWorksInvalidContactTypeException une:
                    // Return 400
                    error = "Invalid ContactTypeID.";
                    actionResult = new BadRequestObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                case AdventureWorksInvalidAddressTypeException une:
                    // Return 400
                    error = "Invalid AddressTypeID.";
                    actionResult = new BadRequestObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                case AdventureWorksNullEntityObjectException une:
                    // Return 404
                    error = "Can't find entity; invalid primary key value given!";
                    actionResult = new NotFoundObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                case AdventureWorksInvalidStateProvinceIdException une:
                    // Return 400
                    error = "Invalid state or province code.";
                    actionResult = new BadRequestObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                case AdventureWorksInvalidPhoneTypeException une:
                    // Return 400
                    error = "Invalid phone type ID.";
                    actionResult = new BadRequestObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                case AdventureWorksException awe:
                    // Return 400
                    error = "AdventureWorksException";
                    actionResult = new BadRequestObjectResult(new { error = error, message = message, StackTrace = stackTrace });
                    _logger.LogError($"{error} : {message}");
                    break;
                default:
                    error = "General Error";
                    actionResult = new ObjectResult(new { error = error, message = message, StackTrace = stackTrace })
                    {
                        StatusCode = 500
                    };
                    _logger.LogError($"{error} : {message}");
                    break;
            }

            context.Result = actionResult;
        }
    }
}