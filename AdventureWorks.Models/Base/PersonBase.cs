using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.CustomTypes;

namespace AdventureWorks.Models.Base
{
    public class PersonBase : EntityBase
    {
        [Key, Required, Display(Name="ID"), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        [Required, DataType(DataType.Text), StringLength(2, ErrorMessage = "PersonType has exactly two characters.")]
        public virtual string PersonType { get; set; }

        public bool IsEasternNameStyle { get; set; } = false;

        [DataType(DataType.Text), StringLength(8, ErrorMessage = "Title has a maximum of 8 characters.")]
        public string Title { get; set; }

        [Required, DataType(DataType.Text), Display(Name="First Name")]
        [StringLength(50, ErrorMessage = "First name has a maximum length of 50 characters.")]
        public string FirstName { get; set; }

        [DataType(DataType.Text),Display(Name="Middle Name")]
        [StringLength(50, ErrorMessage = "Middle name has a maximum length of 50 characters.")]
        public string MiddleName { get; set; }

        [Required, DataType(DataType.Text), Display(Name="Last Name")]
        [StringLength(50, ErrorMessage = "Last name has a maximum length of 50 characters.")]
        public string LastName { get; set; }

        [DataType(DataType.Text), StringLength(10, ErrorMessage = "Suffix has a maximum length of 10 characters.")]
        public string Suffix { get; set; }

        [Required, Display(Name="Email Promo Preference")]
        public EmailPromoPreference EmailPromotion { get; set; } = EmailPromoPreference.NoPromotions;

        public string AdditionalContactInfo { get; set; }

        public string Demographics { get; set; }        
    }
}