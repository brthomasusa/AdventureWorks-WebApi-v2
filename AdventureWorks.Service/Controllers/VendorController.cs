using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using LoggerService;

namespace AdventureWorks.Service.Controllers
{
    [Route("api/vendors")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private const string CLASSNAME = "VendorsController";

        private ILoggerManager _logger;
        private IRepositoryCollection _repository;

        public VendorsController(ILoggerManager logger, IRepositoryCollection repository)
        {
            _logger = logger;
            _repository = repository;
            _logger.LogInfo($"{CLASSNAME}.VendorsController");
        }

        [HttpGet]
        public IActionResult GetAllVendors()
            => Ok(_repository.Vendor.GetVendors(new VendorParameters { PageNumber = 1, PageSize = 10 }));

        [HttpGet("{id}", Name = "VendorById")]
        public IActionResult GetVendorByID(int id)
            => Ok(_repository.Vendor.GetVendorByID(id));

        [HttpGet("{id}/details")]
        public IActionResult GetVendorByIdWithDetails(int id)
            => Ok(_repository.Vendor.GetVendorWithDetails(id));


        [HttpPost]
        public IActionResult CreateVendor([FromBody] VendorDomainObj vendor)
        {
            _repository.Vendor.CreateVendor(vendor);
            return CreatedAtRoute("VendorById", new { id = vendor.BusinessEntityID }, vendor);
        }

        [HttpPut]
        public IActionResult UpdateVendor([FromBody] VendorDomainObj vendor)
        {
            _repository.Vendor.UpdateVendor(vendor);
            return NoContent();
        }

        [HttpDelete]
        public IActionResult DeleteVendor([FromBody] VendorDomainObj vendor)
        {
            _repository.Vendor.DeleteVendor(vendor);
            return NoContent();
        }

    }
}