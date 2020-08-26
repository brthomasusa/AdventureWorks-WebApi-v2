using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.DomainModels;
using LoggerService;

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
        public IActionResult GetContactsForVendor(int vendorId)
            => Ok(_repository.Contact.GetContacts(vendorId, new ContactParameters { PageNumber = 1, PageSize = 10 }));

        [HttpGet("contact/{contactID}", Name = "GetVendorContactByID")]
        public IActionResult GetVendorContactByID(int contactID)
            => Ok(_repository.Contact.GetContactByID(contactID));

        [HttpGet("contact/{contactID}/details")]
        public IActionResult GetVendorContactByIDWithDetails(int contactID)
            => Ok(_repository.Contact.GetContactByIDWithDetails(contactID));

        [HttpPost("contact")]
        public IActionResult CreateVendorContact([FromBody] ContactDomainObj contact)
        {
            _repository.Contact.CreateContact(contact);
            return CreatedAtRoute(nameof(GetVendorContactByID), new { contactID = contact.BusinessEntityID }, contact);
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
    }
}