using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Models.Extensions
{
    public static class AddressExtension
    {
        public static void Map(this Address entityObj, AddressDomainObj domainObj)
        {
            entityObj.AddressID = domainObj.AddressID;
            entityObj.AddressLine1 = domainObj.AddressLine1;
            entityObj.AddressLine2 = domainObj.AddressLine2;
            entityObj.City = domainObj.City;
            entityObj.StateProvinceID = domainObj.StateProvinceID;
            entityObj.PostalCode = domainObj.PostalCode;
            entityObj.SpatialLocation = domainObj.SpatialLocation;
        }
    }
}