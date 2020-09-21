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
    public class VendorAddressController : ControllerBase
    {
        private const string CLASSNAME = "VendorAddressController";

        private ILoggerManager _logger;
        private IRepositoryCollection _repository;

        public VendorAddressController(ILoggerManager logger, IRepositoryCollection repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{vendorId}/addresses")]
        public async Task<IActionResult> GetAddressesForVendor(int vendorId, [FromQuery] AddressParameters addressParameters)
        {
            var addresses = await _repository.Address.GetAddresses(vendorId, addressParameters);

            var metadata = new
            {
                addresses.TotalCount,
                addresses.PageSize,
                addresses.CurrentPage,
                addresses.TotalPages,
                addresses.HasNext,
                addresses.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(addresses);
        }


        [HttpGet("address/{addressID}", Name = "GetVendorAddressByID")]
        public IActionResult GetVendorContactByID(int addressID)
            => Ok(_repository.Address.GetAddressByID(addressID));

        [HttpPost("address")]
        public IActionResult CreateVendorAddress([FromBody] AddressDomainObj address)
        {
            _repository.Address.CreateAddress(address);
            return CreatedAtRoute("GetVendorAddressByID", new { addressID = address.AddressID }, address);
        }

        [HttpPut("address")]
        public IActionResult UpdateVendorAddress([FromBody] AddressDomainObj address)
        {
            _repository.Address.UpdateAddress(address);
            return NoContent();
        }

        [HttpDelete("address")]
        public IActionResult DeleteVendorAddress([FromBody] AddressDomainObj address)
        {
            _repository.Address.DeleteAddress(address);
            return NoContent();
        }
    }
}