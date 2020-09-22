using System.Linq;
using System.Threading.Tasks;
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

        public async Task<PagedList<AddressDomainObj>> GetAddresses(int entityID, AddressParameters addressParameters)
        {
            var pagedList = await PagedList<AddressDomainObj>.ToPagedList(
                DbContext.AddressDomainObj
                    .Where(address => address.ParentEntityID == entityID)
                    .AsQueryable()
                    .OrderBy(a => a.StateProvinceID)
                    .ThenBy(a => a.City)
                    .ThenBy(a => a.AddressLine1),
                addressParameters.PageNumber,
                addressParameters.PageSize);

            return pagedList;
        }

        public async Task<AddressDomainObj> GetAddressByID(int addressID)
        {
            return await DbContext.AddressDomainObj.Where(address => address.AddressID == addressID)
                .FirstOrDefaultAsync();
        }

        public async Task CreateAddress(AddressDomainObj addressDomainObj)
        {
            await DoDatabaseValidation(addressDomainObj);

            var address = new Address { };
            address.Map(addressDomainObj);
            address.BusinessEntityAddressObj = new BusinessEntityAddress
            {
                BusinessEntityID = addressDomainObj.ParentEntityID,
                AddressTypeID = addressDomainObj.AddressTypeID
            };

            DbContext.Address.Add(address);
            await Save();

            addressDomainObj.AddressID = address.AddressID;
        }

        public async Task UpdateAddress(AddressDomainObj addressDomainObj)
        {
            if (await DbContext.Address.FindAsync(addressDomainObj.AddressID) == null)
            {
                string msg = $"Error: Update failed; unable to locate an address in the database with ID '{addressDomainObj.AddressID}'.";
                RepoLogger.LogError(CLASSNAME + ".UpdateAddress " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            await DoDatabaseValidation(addressDomainObj);

            ExecuteInATransaction(DoWork);

            async void DoWork()
            {
                var address = await DbContext.Address
                    .Where(a => a.AddressID == addressDomainObj.AddressID)
                    .Include(a => a.BusinessEntityAddressObj)
                    .FirstOrDefaultAsync();

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

                await Save();
            }
        }

        public async Task DeleteAddress(AddressDomainObj addressDomainObj)
        {
            if (await DbContext.Address.FindAsync(addressDomainObj.AddressID) == null)
            {
                string msg = $"Error: Update failed; unable to locate an address in the database with ID '{addressDomainObj.AddressID}'.";
                RepoLogger.LogError(CLASSNAME + ".DeleteAddress " + msg);
                throw new AdventureWorksNullEntityObjectException(msg);
            }

            ExecuteInATransaction(DoWork);

            async void DoWork()
            {
                var bea = await DbContext.BusinessEntityAddress
                    .FindAsync(addressDomainObj.ParentEntityID, addressDomainObj.AddressID, addressDomainObj.AddressTypeID);

                DbContext.BusinessEntityAddress.Remove(bea);
                await Save();

                var address = await DbContext.Address.FindAsync(addressDomainObj.AddressID);
                DbContext.Address.Remove(address);
                await Save();
            }
        }

        private async Task DoDatabaseValidation(AddressDomainObj addressDomainObj)
        {
            if (await IsValidAddressTypeID(addressDomainObj.AddressTypeID) == false)
            {
                var msg = "Error: Invalid address type detected.";
                RepoLogger.LogError(CLASSNAME + ".DoDatabaseValidation " + msg);
                throw new AdventureWorksInvalidAddressTypeException(msg);
            }

            if (await IsValidParentEntityID(addressDomainObj.ParentEntityID) == false)
            {
                var msg = "Error: Unable to determine the entity that this address belongs to.";
                RepoLogger.LogError(CLASSNAME + ".DoDatabaseValidation " + msg);
                throw new AdventureWorksInvalidEntityIdException(msg);
            }

            if (await IsValidStateProvinceID(addressDomainObj.StateProvinceID) == false)
            {
                var msg = "Error: Invalid state/province ID detected.";
                RepoLogger.LogError(CLASSNAME + ".DoDatabaseValidation " + msg);
                throw new AdventureWorksInvalidStateProvinceIdException(msg);
            }
        }

        private async Task<bool> IsValidAddressTypeID(int addressTypeID)
        {
            return await DbContext.AddressType.Where(ct => ct.AddressTypeID == addressTypeID).AnyAsync();
        }

        private async Task<bool> IsValidParentEntityID(int entityID)
        {
            return await DbContext.BusinessEntity.Where(v => v.BusinessEntityID == entityID).AnyAsync();
        }

        private async Task<bool> IsValidStateProvinceID(int stateProvinceID)
        {
            return await DbContext.StateProvince.Where(s => s.StateProvinceID == stateProvinceID).AnyAsync();
        }
    }
}