using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Purchasing;

namespace AdventureWorks.Models.Extensions
{
    public static class VendorExtension
    {
        public static void Map(this Vendor entityObj, VendorDomainObj domainObj)
        {
            entityObj.BusinessEntityID = domainObj.BusinessEntityID;
            entityObj.AccountNumber = domainObj.AccountNumber;
            entityObj.Name = domainObj.Name;
            entityObj.CreditRating = domainObj.CreditRating;
            entityObj.PreferredVendor = domainObj.PreferredVendor;
            entityObj.PurchasingWebServiceURL = domainObj.PurchasingWebServiceURL;
            entityObj.IsActive = domainObj.IsActive;
        }
    }
}