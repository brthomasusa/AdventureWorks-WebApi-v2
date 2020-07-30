using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.ViewModel;
using PersonClass = AdventureWorks.Models.Person.Person;

namespace AdventureWorks.Dal.Repositories.Interfaces.Purchasing
{
    public interface IVendorRepo : IRepo<Vendor>
    {
        IEnumerable<VendorContactViewModel> GetVendorContactViewModels(int vendorID);

        IEnumerable<VendorViewModel> GetAllVendorViewModels();

        VendorViewModel FindVendorViewModel(Expression<Func<VendorViewModel, bool>> predicate);

        IEnumerable<AddressViewModel> GetVendorAddressViewModelsForOneVendor(int vendorID);

        IEnumerable<PersonClass> GetVendorContacts(int vendorID);

        PersonClass GetVendorContact(int personID);

        IEnumerable<Address> GetVendorAddresses(int vendorID);

        Address GetVendorAddress(int addressID);

        int Add(Vendor vendor);

        int Add(Vendor vendor, PersonClass vendorContact, Address vendorAddress);

        int AddVendorAddress(int vendorID, Address address);

        int AddVendorContact(int vendorID, int contactTypeID, PersonClass contact);

        int UpdateVendorAddress(Address address);

        int UpdateVendorContact(PersonClass contact);

        int DeleteVendorAddress(int addressID);

        int DeleteVendorContact(BusinessEntityContact bizEntityContact);

        int DeleteVendorContact(int vendorID, int personID, int contactTypeID);
    }
}