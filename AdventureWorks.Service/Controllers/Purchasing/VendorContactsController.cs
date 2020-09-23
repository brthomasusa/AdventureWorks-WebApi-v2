using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Person;
using LoggerService;
using Newtonsoft.Json;

namespace AdventureWorks.Service.Controllers
{
    [Route("api/vendors")]
    [ApiController]
    public class VendorContactsController : ControllerBase
    {
        private const string CLASSNAME = "VendorContactsController";

        private ILoggerManager _logger;
        private IRepositoryCollection _repository;

        public VendorContactsController(ILoggerManager logger, IRepositoryCollection repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("{vendorId}/contact")]
        public async Task<IActionResult> GetContactsForVendor(int vendorId, [FromQuery] ContactParameters contactParameters)
        {
            var contacts = await _repository.Contact.GetContacts(vendorId, contactParameters);

            var metadata = new
            {
                contacts.TotalCount,
                contacts.PageSize,
                contacts.CurrentPage,
                contacts.TotalPages,
                contacts.HasNext,
                contacts.HasPrevious
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(metadata));

            return Ok(contacts);
        }


        [HttpGet("contact/{contactID}", Name = "GetVendorContactByID")]
        public async Task<IActionResult> GetVendorContactByID(int contactID)
        {
            var contact = await _repository.Contact.GetContactByID(contactID);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpGet("contact/{contactID}/phones")]
        public async Task<IActionResult> GetVendorContactByIDWithPhones(int contactID)
            => Ok(await _repository.Contact.GetContactByIDWithPhones(contactID));

        [HttpGet("contact/phone/{entityID}/{phoneNumber}/{phoneTypeID}", Name = "GetVendorContactPhone")]
        public async Task<IActionResult> GetVendorContactPhone(int entityID, string phoneNumber, int phoneTypeID)
            => Ok(await _repository.Telephone.GetPhoneByID(entityID, phoneNumber, phoneTypeID));

        [HttpPost("contact")]
        public async Task<IActionResult> CreateVendorContact([FromBody] ContactDomainObj contact)
        {
            await _repository.Contact.CreateContact(contact);
            return CreatedAtRoute(nameof(GetVendorContactByID), new { contactID = contact.BusinessEntityID }, contact);
        }

        [HttpPost("contact/phone")]
        public async Task<IActionResult> CreateVendorContactPhone([FromBody] PersonPhone phone)
        {
            await _repository.Telephone.CreatePhone(phone);

            return CreatedAtRoute(nameof(GetVendorContactPhone),
                new { entityID = phone.BusinessEntityID, phoneNumber = phone.PhoneNumber, phoneTypeID = phone.PhoneNumberTypeID }
                , phone);
        }

        [HttpPut("contact")]
        public async Task<IActionResult> UpdateVendorContact([FromBody] ContactDomainObj contact)
        {
            await _repository.Contact.UpdateContact(contact);
            return NoContent();
        }

        [HttpDelete("contact")]
        public async Task<IActionResult> DeleteVendorContact([FromBody] ContactDomainObj contact)
        {
            await _repository.Contact.DeleteContact(contact);
            return NoContent();
        }

        [HttpDelete("contact/phone")]
        public async Task<IActionResult> DeleteVendorContactPhone([FromBody] PersonPhone phone)
        {
            await _repository.Telephone.DeletePhone(phone);
            return NoContent();
        }
    }
}