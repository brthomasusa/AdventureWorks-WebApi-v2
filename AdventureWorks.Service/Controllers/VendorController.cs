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
        private readonly IVendorRepo _vendorRepo;
        private ILoggerManager _logger;

        public VendorsController(IVendorRepo repo, ILoggerManager logger)
        {
            _vendorRepo = repo;
            _logger = logger;
            _logger.LogInfo("VendorsController: logging is working.");
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<Vendor>> GetAllVendors() => Ok(_vendorRepo.GetAll());

        [HttpGet("{vendorID}", Name = "GetById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Vendor> GetById(int vendorID)
        {
            var vendor = _vendorRepo.Find(vendorID);

            if (vendor == null)
            {
                return NotFound();
            }

            return Ok(vendor);
        }

        [HttpGet("{vendorID}/address")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<Address>> GetVendorAddresses(int vendorID)
            => Ok(_vendorRepo.GetVendorAddresses(vendorID));

        [HttpGet("address/{addressID}", Name = "GetVendorAddress")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Address> GetVendorAddress(int addressID)
        {
            var address = _vendorRepo.GetVendorAddress(addressID);

            if (address == null)
            {
                return NotFound();
            }

            return Ok(address);
        }

        [HttpGet("{vendorID}/contact")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<PersonClass>> GetVendorContacts(int vendorID)
            => Ok(_vendorRepo.GetVendorContacts(vendorID));

        [HttpGet("contact/{personID}", Name = "GetVendorContact")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<PersonClass> GetVendorContact(int personID)
        {
            PersonClass contact = _vendorRepo.GetVendorContact(personID);

            if (contact == null)
            {
                return NotFound();
            }

            return Ok(contact);
        }

        [HttpGet("ViewModels")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<VendorViewModel>> GetVendorViewModels()
            => Ok(_vendorRepo.GetVendorViewModels());

        [HttpGet("ViewModels/{vendorID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<VendorViewModel> GetVendorViewModel(int vendorID)
        {
            var vendor = _vendorRepo.GetVendorViewModel(vendorID);
            if (vendor != null)
            {
                return Ok(vendor);
            }

            return NotFound();
        }

        [HttpGet("{vendorID}/AddressViewModel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<AddressViewModel>> GetVendorAddressViewModels(int vendorID)
            => Ok(_vendorRepo.GetVendorAddressViewModels(vendorID));

        [HttpGet("AddressViewModel/{addressID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<AddressViewModel> GetVendorAddressViewModel(int addressID)
        {
            var address = _vendorRepo.GetAddressViewModel(addressID);
            if (address != null)
            {
                return Ok(address);
            }

            return NotFound();
        }

        [HttpGet("{vendorID}/ContactViewModel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<VendorContactViewModel>> GetVendorContactViewModels(int vendorID)
            => Ok(_vendorRepo.GetVendorContactViewModels(vendorID));

        [HttpGet("ContactViewModel/{personID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<PersonClass> GetVendorContactViewModel(int personID)
        {
            var contact = _vendorRepo.GetVendorContactViewModel(personID);
            if (contact != null)
            {
                return Ok(contact);
            }

            return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<Vendor> CreateVendor([FromBody] Vendor vendor)
        {
            if (vendor == null)
            {
                return BadRequest();
            }

            var vendorID = _vendorRepo.Add(vendor);

            return CreatedAtAction(nameof(GetById), new { vendorID = vendorID }, vendor);
        }

        [HttpPost("{vendorID}/VendorContact/{contactTypeID}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<PersonClass> CreateVendorContact([FromBody] PersonClass contact, int vendorID, int contactTypeID)
        {
            if (contact == null)
            {
                return BadRequest();
            }

            _vendorRepo.AddVendorContact(vendorID, contactTypeID, contact);

            return CreatedAtAction(nameof(GetVendorContact), new { personID = contact.BusinessEntityID }, contact);
        }













    }
}