using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Models.DomainModels
{
    public class ContactDomainObj : PersonBase, IValidatableObject
    {
        [DataType(DataType.EmailAddress), Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string EmailPasswordHash { get; set; }

        [Required]
        public string EmailPasswordSalt { get; set; }

        public int ContactTypeID { get; set; }

        public int ParentEntityID { get; set; }

        [NotMapped]
        public List<PersonPhone> Phones { get; set; } = new List<PersonPhone>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PersonType != "VC")
            {
                yield return new ValidationResult("PersonType should be VC (Vendor Contact).");
            }
        }
    }
}