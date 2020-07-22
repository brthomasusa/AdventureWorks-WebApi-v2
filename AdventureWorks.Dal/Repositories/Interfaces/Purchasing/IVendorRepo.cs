using System.Collections.Generic;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.ViewModel;
using PersonClass = AdventureWorks.Models.Person.Person;

namespace AdventureWorks.Dal.Repositories.Interfaces.Purchasing
{
    public interface IVendorRepo : IRepo<Vendor>
    {
        IEnumerable<VendorContact> GetVendorContactViewModelsForAllVendors();

        IEnumerable<VendorContact> GetVendorContactViewModelsForOneVendor(int vendorID);

        IEnumerable<VendorAddress> GetVendorAddressViewModelsForAllVendors();

        VendorAddress GetVendorAddressViewModelForOneVendor(int vendorID);

        IEnumerable<PersonClass> GetVendorContacts(int vendorID);

        PersonClass GetVendorContact(int personID);

        PersonClass GetVendorContact(int vendorID, int personID, int contactTypeID);

        IEnumerable<Address> GetVendorAddresses(int vendorID);

        Address GetVendorAddress(int addressID);

        int Add(Vendor vendor);

        int Add(Vendor vendor, PersonClass vendorContact, Address vendorAddress);

        int AddVendorAddress(int vendorID, int addressTypeID, Address address);

        int AddVendorContact(int vendorID, int contactTypeID, PersonClass contact);

        int UpdateVendorAddress(Address address);

        int UpdateVendorContact(PersonClass contact);

        int DeleteVendorAddress(int addressID);

        int DeleteVendorContact(BusinessEntityContact bizEntityContact);

        int DeleteVendorContact(int vendorID, int personID, int contactTypeID);
    }
}