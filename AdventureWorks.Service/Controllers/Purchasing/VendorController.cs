using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using LoggerService;
using Newtonsoft.Json;

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
        }

        [HttpGet]
        public async Task<IActionResult> GetAllVendors([FromQuery] VendorParameters vendorParameters)
        {
            if (vendorParameters.VendorStatus != null && !vendorParameters.IsValidVendorStatus)
            {
                return BadRequest(new { message = "Valid vendor status codes are 'Active', 'Inactive', and 'All'." });
            }

            var vendors = await _repository.Vendor.GetVendors(vendorParameters);

            var metadata = new
            {
                vendors.TotalCount,
                vendors.PageSize,
                vendors.CurrentPage,
                vendors.TotalPages,
                vendors.HasNext,
                vendors.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(vendors);
        }

        [HttpGet("{id}", Name = "VendorById")]
        public IActionResult GetVendorByID(int id)
            => Ok(_repository.Vendor.GetVendorByID(id));

        [HttpGet("{id}/details")]
        public async Task<IActionResult> GetVendorByIdWithDetails(int id)
            => Ok(await _repository.Vendor.GetVendorWithDetails(id));


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