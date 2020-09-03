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
        public IActionResult GetContactsForVendor(int vendorId, [FromQuery] ContactParameters contactParameters)
        {
            var contacts = _repository.Contact.GetContacts(vendorId, contactParameters);

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
        public IActionResult GetVendorContactByID(int contactID)
        {
            var contact = _repository.Contact.GetContactByID(contactID);
            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpGet("contact/{contactID}/phones")]
        public IActionResult GetVendorContactByIDWithPhones(int contactID)
            => Ok(_repository.Contact.GetContactByIDWithPhones(contactID));

        [HttpGet("contact/phone/{entityID}/{phoneNumber}/{phoneTypeID}", Name = "GetVendorContactPhone")]
        public IActionResult GetVendorContactPhone(int entityID, string phoneNumber, int phoneTypeID)
            => Ok(_repository.Telephone.GetPhoneByID(entityID, phoneNumber, phoneTypeID));

        [HttpPost("contact")]
        public IActionResult CreateVendorContact([FromBody] ContactDomainObj contact)
        {
            _repository.Contact.CreateContact(contact);
            return CreatedAtRoute(nameof(GetVendorContactByID), new { contactID = contact.BusinessEntityID }, contact);
        }

        [HttpPost("contact/phone")]
        public IActionResult CreateVendorContactPhone([FromBody] PersonPhone phone)
        {
            _repository.Telephone.CreatePhone(phone);

            return CreatedAtRoute(nameof(GetVendorContactPhone),
                new { entityID = phone.BusinessEntityID, phoneNumber = phone.PhoneNumber, phoneTypeID = phone.PhoneNumberTypeID }
                , phone);
        }

        [HttpPut("contact")]
        public IActionResult UpdateVendorContact([FromBody] ContactDomainObj contact)
        {
            _repository.Contact.UpdateContact(contact);
            return NoContent();
        }

        [HttpDelete("contact")]
        public IActionResult DeleteVendorContact([FromBody] ContactDomainObj contact)
        {
            _repository.Contact.DeleteContact(contact);
            return NoContent();
        }

        [HttpDelete("contact/phone")]
        public IActionResult DeleteVendorContactPhone([FromBody] PersonPhone phone)
        {
            _repository.Telephone.DeletePhone(phone);
            return NoContent();
        }
    }
}