using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.ViewModel;
using PersonClass = AdventureWorks.Models.Person.Person;

namespace AdventureWorks.Service.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]    
    public class VendorController : ControllerBase
    {
        private readonly IVendorRepo _vendorRepo;

        public VendorController(IVendorRepo repo)
        {
            _vendorRepo = repo;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]  
        public ActionResult<IList<Vendor>> GetAllVendors() => Ok(_vendorRepo.GetAll());            

        [HttpGet("{vendorID}", Name="GetById")]
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

        [HttpGet("address/{addressID}", Name="GetVendorAddress")]
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

        [HttpGet("contact/{personID}", Name="GetVendorContact")]
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

















    }
}