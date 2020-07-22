using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Purchasing;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Purchasing;
using AdventureWorks.Models.ViewModel;
using PersonClass = AdventureWorks.Models.Person.Person;

namespace AdventureWorks.Dal.Repositories.Purchasing
{
    public class VendorRepo : RepoBase<Vendor>, IVendorRepo
    {
        public VendorRepo(AdventureWorksContext context) : base(context) { }

        internal VendorRepo(DbContextOptions<AdventureWorksContext> options) : base(options) { }

        public IEnumerable<VendorContact> GetVendorContactViewModelsForAllVendors()
            => Context.VendorContact.ToList();

        public IEnumerable<VendorContact> GetVendorContactViewModelsForOneVendor(int vendorID)
            => Context.VendorContact
                .Where(v => v.BusinessEntityID == vendorID)
                .ToList();

        public IEnumerable<VendorAddress> GetVendorAddressViewModelsForAllVendors()
            => Context.VendorAddress.ToList();


        public VendorAddress GetVendorAddressViewModelForOneVendor(int vendorID)
            => Context.VendorAddress
                .Where(v => v.BusinessEntityID == vendorID)
                .FirstOrDefault<VendorAddress>();

        public override Vendor Find(Expression<Func<Vendor, bool>> predicate)
            => Table.Where(predicate)
                .Include(v => v.BusinessEntityAddresses)
                .Include(v => v.BusinessEntityContacts)
                .FirstOrDefault();

        public IEnumerable<Address> GetVendorAddresses(int vendorID)
        {
            var vendor = Table
                .Where(v => v.BusinessEntityID == vendorID)
                .Include(v => v.BusinessEntityAddresses)
                    .ThenInclude(bea => bea.AddressNavigation)
                .FirstOrDefault<Vendor>();

            var addresses = new List<Address>();

            foreach (BusinessEntityAddress bea in vendor.BusinessEntityAddresses)
            {
                addresses.Add(bea.AddressNavigation);
            }

            return addresses;
        }

        public Address GetVendorAddress(int addressID)
            => Context.Address
                .Where(a => a.AddressID == addressID)
                .Include(bea => bea.BusinessEntityAddressObj)
                .FirstOrDefault();

        public IEnumerable<PersonClass> GetVendorContacts(int vendorID)
        {
            var vendor = Table
                .Where(v => v.BusinessEntityID == vendorID)
                .Include(v => v.BusinessEntityContacts)
                    .ThenInclude(bec => bec.PersonNavigation)
                .FirstOrDefault<Vendor>();

            var contacts = new List<PersonClass>();

            foreach (var bizEntityContact in vendor.BusinessEntityContacts)
            {
                contacts.Add(bizEntityContact.PersonNavigation);
            }

            return contacts;
        }

        public PersonClass GetVendorContact(int personID)
            => Context.Person.Where(p => p.BusinessEntityID == personID).FirstOrDefault();

        public PersonClass GetVendorContact(int vendorID, int personID, int contactTypeID)
        {
            var vendor = Table
                .Where(v => v.BusinessEntityID == vendorID)
                .Include(bec => bec.BusinessEntityContacts)
                .FirstOrDefault();

            var bizEntityContact = vendor.BusinessEntityContacts
                .ToList()
                .Find(bec => bec.PersonID == personID && bec.ContactTypeID == contactTypeID);

            return Context.Person
                .Where(p => p.BusinessEntityID == bizEntityContact.PersonID)
                .Include(p => p.EmailAddressObj)
                .Include(p => p.PasswordObj)
                .Include(p => p.Phones)
                .FirstOrDefault();
        }

        public int Add(Vendor vendor)
        {
            var bizEntity = new BusinessEntity { };
            Context.BusinessEntity.Add(bizEntity);
            SaveChanges();

            vendor.BusinessEntityID = bizEntity.BusinessEntityID;
            base.Add(vendor);
            SaveChanges();

            return vendor.BusinessEntityID;
        }

        public int Add(Vendor vendor, PersonClass vendorContact, Address vendorAddress)
        {
            ExecuteInATransaction(DoWork);
            return vendor.BusinessEntityID;

            void DoWork()
            {
                // Get new primary keys for Person and Vendor entity objects
                var personEntity = new BusinessEntity { };
                var vendorEntity = new BusinessEntity { };
                Context.BusinessEntity.AddRange(new List<BusinessEntity> { personEntity, vendorEntity });
                SaveChanges();

                var personID = personEntity.BusinessEntityID;
                var vendorID = vendorEntity.BusinessEntityID;

                // Add a Vendor, a Person, and an Address to the database
                vendor.BusinessEntityID = vendorID;
                vendorContact.BusinessEntityID = personID;

                Context.Person.Add(vendorContact);
                Context.Vendor.Add(vendor);
                Context.Address.Add(vendorAddress);
                SaveChanges();

                // Link Address to Vendor                
                Context.BusinessEntityAddress.Add(new BusinessEntityAddress
                {
                    BusinessEntityID = vendorID,
                    AddressID = vendorAddress.AddressID,
                    AddressTypeID = 3
                });

                // Link Contact (PersonClass) to Vendor
                vendor.BusinessEntityContacts.Add(new BusinessEntityContact
                {
                    BusinessEntityID = vendorID,
                    PersonID = personID,
                    ContactTypeID = 17
                });

                SaveChanges();
            }
        }

        public int AddVendorAddress(int vendorID, int addressTypeID, Address address)
        {
            ExecuteInATransaction(DoWork);
            return address.AddressID;

            void DoWork()
            {
                Context.Address.Add(address);
                SaveChanges();

                var vendor = Table.Find(vendorID);
                vendor.BusinessEntityAddresses.Add(new BusinessEntityAddress
                {
                    BusinessEntityID = vendor.BusinessEntityID,
                    AddressID = address.AddressID,
                    AddressTypeID = addressTypeID
                });
                SaveChanges();
            }
        }

        public int AddVendorContact(int vendorID, int contactTypeID, PersonClass contact)
        {
            ExecuteInATransaction(DoWork);
            return contact.BusinessEntityID;

            void DoWork()
            {
                // Get next available primary key to use for Person.BusinessEntityID
                var bizEntity = new BusinessEntity { };
                Context.BusinessEntity.Add(bizEntity);
                SaveChanges();

                // Set Person.Person primary key field
                contact.BusinessEntityID = bizEntity.BusinessEntityID;
                Context.Person.Add(contact);
                SaveChanges();

                // Get the Purchasing.Vendor for this contact
                var vendor = Table.Find(vendorID);

                // Create link record between Purchasing.Vendor and Person.Person
                vendor.BusinessEntityContacts.Add(new BusinessEntityContact
                {
                    BusinessEntityID = vendorID,
                    PersonID = contact.BusinessEntityID,
                    ContactTypeID = contactTypeID
                });

                SaveChanges();
            }
        }

        public int UpdateVendorAddress(Address address)
        {
            Context.Address.Update(address);
            SaveChanges();
            return address.AddressID;
        }

        public int UpdateVendorContact(PersonClass contact)
        {
            Context.Person.Update(contact);
            SaveChanges();
            return contact.BusinessEntityID;
        }

        public override int Delete(Vendor entity, bool persist = true)
        {
            entity.IsActive = false;
            Table.Update(entity);
            return persist ? SaveChanges() : 0;
        }

        public override int DeleteRange(IEnumerable<Vendor> entities, bool persist = true)
        {
            foreach (var vendor in entities)
            {
                vendor.IsActive = false;
            }

            Table.UpdateRange(entities);
            return persist ? SaveChanges() : 0;
        }

        public int DeleteVendorAddress(int addressID)
        {
            ExecuteInATransaction(DoWork);
            return 1;

            void DoWork()
            {
                var address = Context.Address
                    .Where(a => a.AddressID == addressID)
                    .Include(a => a.BusinessEntityAddressObj)
                    .FirstOrDefault();

                // Remove the linking record
                Context.BusinessEntityAddress.Remove(address.BusinessEntityAddressObj);
                SaveChanges();

                // Remove the address
                Context.Address.Remove(address);
                SaveChanges();
            }
        }

        public int DeleteVendorContact(BusinessEntityContact bizEntityContact)
        {
            // Just remove the record that links the Vendor to the Person
            Context.BusinessEntityContact.Remove(bizEntityContact);
            return SaveChanges();
        }

        public int DeleteVendorContact(int vendorID, int personID, int contactTypeID)
        {
            var bizEntityContact = Context.BusinessEntityContact
                .Where(bec => bec.BusinessEntityID == vendorID && bec.PersonID == personID && bec.ContactTypeID == contactTypeID)
                .SingleOrDefault();

            Context.BusinessEntityContact.Remove(bizEntityContact);
            return SaveChanges();
        }
    }
}