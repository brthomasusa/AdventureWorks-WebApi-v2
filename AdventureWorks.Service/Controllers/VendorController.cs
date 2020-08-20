using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.ViewModel;
using PersonClass = AdventureWorks.Models.Person.Person;
using LoggerService;

namespace AdventureWorks.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private ILoggerManager _logger;

        public VendorsController(ILoggerManager logger)
        {
            _logger = logger;
        }

    }
}