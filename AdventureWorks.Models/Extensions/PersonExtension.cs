using AdventureWorks.Models.DomainModels;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Models.Extensions
{
    public static class PersonExtension
    {
        public static void Map(this Person.Person entityObj, ContactDomainObj domainObj)
        {
            entityObj.BusinessEntityID = domainObj.BusinessEntityID;
            entityObj.PersonType = domainObj.PersonType;
            entityObj.IsEasternNameStyle = domainObj.IsEasternNameStyle;
            entityObj.Title = domainObj.Title;
            entityObj.FirstName = domainObj.FirstName;
            entityObj.MiddleName = domainObj.MiddleName;
            entityObj.LastName = domainObj.LastName;
            entityObj.Suffix = domainObj.Suffix;
            entityObj.EmailPromotion = domainObj.EmailPromotion;
            entityObj.AdditionalContactInfo = domainObj.AdditionalContactInfo;
            entityObj.Demographics = domainObj.Demographics;
        }

        public static void Map(this Person.Person entityObj, EmployeeDomainObj domainObj)
        {
            entityObj.BusinessEntityID = domainObj.BusinessEntityID;
            entityObj.PersonType = domainObj.PersonType;
            entityObj.IsEasternNameStyle = domainObj.IsEasternNameStyle;
            entityObj.Title = domainObj.Title;
            entityObj.FirstName = domainObj.FirstName;
            entityObj.MiddleName = domainObj.MiddleName;
            entityObj.LastName = domainObj.LastName;
            entityObj.Suffix = domainObj.Suffix;
            entityObj.EmailPromotion = domainObj.EmailPromotion;
            entityObj.AdditionalContactInfo = domainObj.AdditionalContactInfo;
            entityObj.Demographics = domainObj.Demographics;
        }
    }
}