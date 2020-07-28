using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.CustomTypes;
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

        [HttpGet("ContactViewModel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<VendorContact>> GetVendorContactViewModel() 
            => Ok(_vendorRepo.GetVendorContactViewModelsForAllVendors().ToList());

        [HttpGet("ContactViewModel/{vendorID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<IList<VendorContact>> GetVendorContactViewModel(int vendorID)
        {
            IEnumerable<VendorContact> contacts = _vendorRepo.GetVendorContactViewModelsForOneVendor(vendorID).ToList();

            if (contacts == null || contacts.Count() == 0)
            {
                return NotFound();
            }

            return Ok(contacts);
        }

        [HttpGet("AddressViewModel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<VendorAddress>> GetVendorAddressViewModel() 
            => Ok(_vendorRepo.GetVendorAddressViewModelsForAllVendors());

        [HttpGet("AddressViewModel/{vendorID}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]   
        public ActionResult<VendorContact> GetVendorAddressViewModel(int vendorID)
        {
            var vendorAddress = _vendorRepo.GetVendorAddressViewModelForOneVendor(vendorID);

            if (vendorAddress == null)
            {
                return NotFound();
            }

            return Ok(vendorAddress);
        }  

        [HttpGet(Name="GetAllVendors")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]  
        public ActionResult<IList<Vendor>> GetAllVendors() 
            => Ok(_vendorRepo.GetAll());

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

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<Vendor> CreateVendor(Vendor vendor)
        {
            if (vendor == null)
            {
                return BadRequest();
            }
            
            var vendorID = _vendorRepo.Add(vendor);

            return CreatedAtAction(nameof(GetById), new { vendorID = vendorID },vendor);
        }  

        [HttpPut("{vendorID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<Vendor> UpdateVendor(int vendorID, Vendor vendor)
        {
            if (vendor == null)
            {
                return BadRequest();
            }
            
            _vendorRepo.Update(vendor);

            return NoContent();            
        }

        [HttpDelete("{vendorID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Vendor> DeleteVendor(int vendorID)
        {
            var vendor = _vendorRepo.Find(vendorID);
            
            if (vendor == null)
            { 
                return NotFound();
            }
            
            _vendorRepo.Delete(vendor);

            return NoContent();            
        }

        [HttpGet("{vendorID}/address")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<Address>> GetVendorAddresses(int vendorID) 
            => Ok(_vendorRepo.GetVendorAddresses(vendorID));
             
        [HttpGet("{vendorID}/contact")]
        [ProducesResponseType(200)]
        [ProducesResponseType(500)]
        public ActionResult<IList<Address>> GetVendorContacts(int vendorID) 
            => Ok(_vendorRepo.GetVendorContacts(vendorID)); 

        [HttpGet("{vendorID}/address/{addressID}", Name="GetVendorAddress")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Address> GetVendorAddress(int vendorID, int addressID)
        {
            var address = _vendorRepo.GetVendorAddress(addressID);
            
            if (address == null)
            { 
                return NotFound();
            }

            return Ok(address);            
        }

        [HttpGet("{vendorID}/contact/{personID}/{contactTypeID}", Name="GetVendorContact")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<PersonClass> GetVendorContact(int vendorID, int personID, int contactTypeID)
        {
            PersonClass contact = _vendorRepo.GetVendorContact(vendorID, personID, contactTypeID);
 
            if (contact == null)
            { 
                return NotFound();
            }

            return Ok(contact);            
        }

        [HttpPost("{vendorID}/address")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<Address> CreateVendorAddress(int vendorID, Address address)
        {
            if (address == null)
            {
                return BadRequest();
            }
            
            var ID = _vendorRepo.AddVendorAddress(vendorID, address);

            return CreatedAtAction(nameof(GetVendorAddress), new { addressID = ID },address);            
        }

        [HttpPost("{vendorID}/contact/{contactTypeID}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<PersonClass> CreateVendorContact(int vendorID, int contactTypeID, PersonClass contact)
        {
            if (contact == null)
            {
                return BadRequest();
            } 

            var ID = _vendorRepo.AddVendorContact(vendorID, contactTypeID, contact);

            return CreatedAtAction(nameof(GetVendorContact), new { vendorID =  vendorID ,personID = ID, contactTypeID = contactTypeID}, contact);                        
        }

        // TODO ChangeVendorContactType action method is needed

        [HttpPut("{vendorID}/address")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<Address> UpdateVendorAddress(int vendorID, Address address)
        {
            if (address == null)
            {
                return BadRequest();
            }
            
            _vendorRepo.UpdateVendorAddress(address);

            return NoContent();            
        } 

        [HttpPut("{vendorID}/contact")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public ActionResult<PersonClass> UpdateVendorContact(int vendorID, PersonClass contact)
        {
            if (contact == null)
            {
                return BadRequest();
            }
            
            _vendorRepo.UpdateVendorContact(contact);

            return NoContent();            
        } 

        [HttpDelete("{vendorID}/address/{addressID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<Address> DeleteVendorAddress(int vendorID, int addressID)
        {            
            _vendorRepo.DeleteVendorAddress(addressID);

            return NoContent();            
        }           

        [HttpDelete("{vendorID}/contact/{personID}/{contactTypeID}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public ActionResult<PersonClass> DeleteVendorContact(int vendorID, int personID, int contactTypeID)
        {                        
            _vendorRepo.DeleteVendorContact(vendorID, personID, contactTypeID);

            return NoContent();            
        } 

    }
}