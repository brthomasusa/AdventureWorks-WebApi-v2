using System.Linq;
using AdventureWorks.Dal.EfCode;
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
            var address = DbContext.Address.Find(addressDomainObj.AddressID);
            address.Map(addressDomainObj);
            DbContext.Address.Update(address);
            Save();
        }

        public void DeleteAddress(AddressDomainObj addressDomainObj)
        {
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
    }
}