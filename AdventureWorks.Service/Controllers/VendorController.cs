using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.DomainModels;
using LoggerService;

namespace AdventureWorks.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private ILoggerManager _logger;
        private IRepositoryCollection _repository;

        public VendorsController(ILoggerManager logger, IRepositoryCollection repository)
        {
            _logger = logger;
            _repository = repository;
        }

    }
}