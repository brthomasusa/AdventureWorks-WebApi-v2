using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.DomainModels;
using LoggerService;

namespace AdventureWorks.Service.Controllers
{
    [Route("api/vendors")]
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

        [HttpGet]
        public IActionResult GetAllVendors()
        {
            var vendors = _repository.Vendor.GetVendors(new VendorParameters { PageNumber = 1, PageSize = 10 });
            return Ok(vendors);
        }

    }
}