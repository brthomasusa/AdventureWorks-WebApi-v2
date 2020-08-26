using System.ComponentModel.DataAnnotations;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Validation;

namespace AdventureWorks.Models.Base
{
    public class PersonBase : EntityBase
    {
        [Required, Display(Name = "ID")]
        public int BusinessEntityID { get; set; }

        [Required]
        public virtual string PersonType { get; set; }

        [Required]
        public bool IsEasternNameStyle { get; set; } = false;

        [StringLength(8, ErrorMessage = "Title has a maximum of 8 characters.")]
        public string Title { get; set; }

        [Required, Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name has a maximum length of 50 characters.")]
        public string FirstName { get; set; }

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Middle name has a maximum length of 50 characters.")]
        public string MiddleName { get; set; }

        [Required, Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name has a maximum length of 50 characters.")]
        public string LastName { get; set; }

        [StringLength(10, ErrorMessage = "Suffix has a maximum length of 10 characters.")]
        public string Suffix { get; set; }

        [Required, EmailPromoPreference, Display(Name = "Email Promo Preference")]
        public EmailPromoPreference EmailPromotion { get; set; } = EmailPromoPreference.NoPromotions;

        public string AdditionalContactInfo { get; set; }

        public string Demographics { get; set; }
    }
}