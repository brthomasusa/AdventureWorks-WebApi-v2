using AdventureWorks.Models.Person;

namespace AdventureWorks.Models.Extensions
{
    public static class PersonPhoneExtension
    {
        public static void Map(this PersonPhone destination, PersonPhone source)
        {
            destination.BusinessEntityID = source.BusinessEntityID;
            destination.PhoneNumber = source.PhoneNumber;
            destination.PhoneNumberTypeID = source.PhoneNumberTypeID;
        }
    }
}