using System.Linq;
using Microsoft.EntityFrameworkCore;
using AdventureWorks.Dal.EfCode;
using AdventureWorks.Dal.Exceptions;
using AdventureWorks.Dal.Repositories.Base;
using AdventureWorks.Dal.Repositories.Interfaces.Person;
using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Extensions;
using AdventureWorks.Models.Person;
using AdventureWorks.Models.Helpers;
using LoggerService;

namespace AdventureWorks.Dal.Repositories.Person
{
    public class AddressRepository : RepositoryBase<AddressDomainObj>, IAddressRepository
    {
        private const string CLASSNAME = "AddressRepository";

        public AddressRepository(AdventureWorksContext context, ILoggerManager logger)
         : base(context, logger) { }

        public PagedList<AddressDomainObj> GetAddresses(int entityID, AddressParameters addressParameters)
        {
            return PagedList<AddressDomainObj>.ToPagedList(
                DbContext.AddressDomainObj
                    .Where(address => address.ParentEntityID == entityID)
                    .AsQueryable(),
                addressParameters.PageNumber,
                addressParameters.PageSize);
        }

        public AddressDomainObj GetAddressByID(int addressID)
        {
            return DbContext.AddressDomainObj.Where(address => address.AddressID == addressID)
                .AsQueryable()
                .FirstOrDefault();
        }

        public void CreateAddress(AddressDomainObj addressDomainObj)
        {
            DoDatabaseValidation(addressDomainObj);

            var address = new Address { };
            address.Map(addressDomainObj);
            address.BusinessEntityAddressObj = new BusinessEntityAddress
            {
                BusinessEntityID = addressDomainObj.ParentEntityID,
                AddressTypeID = addressDomainObj.AddressTypeID
            };

            DbContext.Address.Add(address);
            Save();

            addressDomainObj.AddressID = address.AddressID;
        }

        public void UpdateAddress(AddressDomainObj addressDomainObj)
        {
            if (DbContext.Address.Find(addressDomainObj.AddressID) == null)
            {
                string msg = $"Error: Update failed; unable to locate an address in the database with ID '{addressDomainObj.AddressID}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdateAddress " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            DoDatabaseValidation(addressDomainObj);

            ExecuteInATransaction(DoWork);

            void DoWork()
            {
                var address = DbContext.Address
                    .Where(a => a.AddressID == addressDomainObj.AddressID)
                    .Include(a => a.BusinessEntityAddressObj)
                    .FirstOrDefault();

                if (addressDomainObj.AddressTypeID != address.BusinessEntityAddressObj.AddressTypeID)
                {
                    // AddressTypeID is part of the primary key; it can't be edited. Delete
                    // BusinessEntity record and insert new one with the updated AddressTypeID
                    DbContext.BusinessEntityAddress.Remove(address.BusinessEntityAddressObj);
                    DbContext.BusinessEntityAddress.Add(
                        new BusinessEntityAddress
                        {
                            BusinessEntityID = addressDomainObj.ParentEntityID,
                            AddressID = addressDomainObj.AddressID,
                            AddressTypeID = addressDomainObj.AddressTypeID
                        }
                    );
                }

                address.Map(addressDomainObj);
                DbContext.Address.Update(address);

                Save();
            }
        }

        public void DeleteAddress(AddressDomainObj addressDomainObj)
        {
            if (DbContext.Address.Find(addressDomainObj.AddressID) == null)
            {
                string msg = $"Error: Update failed; unable to locate an address in the database with ID '{addressDomainObj.AddressID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteAddress " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            ExecuteInATransaction(DoWork);

            void DoWork()
            {
                var bea = DbContext.BusinessEntityAddress
                    .Find(addressDomainObj.ParentEntityID, addressDomainObj.AddressID, addressDomainObj.AddressTypeID);

                DbContext.BusinessEntityAddress.Remove(bea);
                Save();

                var address = DbContext.Address.Find(addressDomainObj.AddressID);
                DbContext.Address.Remove(address);
                Save();
            }
        }

        private void DoDatabaseValidation(AddressDomainObj addressDomainObj)
        {
            if (!IsValidAddressTypeID(addressDomainObj.AddressTypeID))
            {
                var msg = "Error: Invalid address type detected.";
                RepoLogger.LogError(CLASSNAME + ".DoDatabaseValidation " + msg);
                throw new AdventureWorksInvalidAddressTypeException(msg);
            }

            if (!IsValidParentEntityID(addressDomainObj.ParentEntityID))
            {
                var msg = "Error: Unable to determine the entity that this address belongs to.";
                RepoLogger.LogError(CLASSNAME + ".DoDatabaseValidation " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }

            if (!IsValidStateProvinceID(addressDomainObj.StateProvinceID))
            {
                var msg = "Error: Invalid state/province ID detected.";
                RepoLogger.LogError(CLASSNAME + ".DoDatabaseValidation " + msg);
                throw new AdventureWorksInvalidStateProvinceIdException(msg);
            }
        }

        private bool IsValidAddressTypeID(int addressTypeID)
        {
            return DbContext.AddressType.Where(ct => ct.AddressTypeID == addressTypeID).Any();
        }

        private bool IsValidParentEntityID(int entityID)
        {
            return DbContext.BusinessEntity.Where(v => v.BusinessEntityID == entityID).Any();
        }

        private bool IsValidStateProvinceID(int stateProvinceID)
        {
            return DbContext.StateProvince.Where(s => s.StateProvinceID == stateProvinceID).Any();
        }
    }
}