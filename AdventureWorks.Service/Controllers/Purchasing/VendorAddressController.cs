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
        public async Task<IActionResult> GetVendorContactByID(int addressID)
            => Ok(await _repository.Address.GetAddressByID(addressID));

        [HttpPost("address")]
        public async Task<IActionResult> CreateVendorAddress([FromBody] AddressDomainObj address)
        {
            await _repository.Address.CreateAddress(address);
            return CreatedAtRoute("GetVendorAddressByID", new { addressID = address.AddressID }, address);
        }

        [HttpPut("address")]
        public async Task<IActionResult> UpdateVendorAddress([FromBody] AddressDomainObj address)
        {
            await _repository.Address.UpdateAddress(address);
            return NoContent();
        }

        [HttpDelete("address")]
        public async Task<IActionResult> DeleteVendorAddress([FromBody] AddressDomainObj address)
        {
            await _repository.Address.DeleteAddress(address);
            return NoContent();
        }
    }
}