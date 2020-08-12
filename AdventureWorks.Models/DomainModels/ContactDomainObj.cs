using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.DomainModels
{
    public class ContactDomainObj : PersonBase
    {
        public int ContactTypeID { get; set; }

        public int ParentEntityID { get; set; }
    }
}