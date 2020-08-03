using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.CustomTypes;

namespace AdventureWorks.Models.ViewModel
{
    public class VendorContactViewModel : PersonBase
    {
        [NotMapped, Display(Name = "Person Type")]
        public string PersonTypeLong
        {
            get
            {
                var personTypeDesc = string.Empty;

                switch (PersonType)
                {
                    case "SC":
                        personTypeDesc = "Store Contact";
                        break;
                    case "IN":
                        personTypeDesc = "Individual (retail) customer";
                        break;
                    case "SP":
                        personTypeDesc = "Sales Person";
                        break;
                    case "EM":
                        personTypeDesc = "Employee (non-sales)";
                        break;
                    case "VC":
                        personTypeDesc = "Vendor Contact";
                        break;
                    case "GC":
                        personTypeDesc = "General Contact";
                        break;
                }

                return personTypeDesc;
            }
        }

        [Required]
        public int EmailAddressID { get; set; }

        [Required, DataType(DataType.EmailAddress), Display(Name = "Email Address")]
        [StringLength(50, ErrorMessage = "Email address has a maximum length of 50 characters.")]
        public string EmailAddress { get; set; }

        [Required, Display(Name = "Email Password Hash")]
        [StringLength(128, ErrorMessage = "Password hash has a maximum length of 128 characters.")]
        public string EmailPasswordHash { get; set; }

        [Required, Display(Name = "Email Password Salt")]
        [StringLength(10, ErrorMessage = "Password salt has a maximum length of 10 characters.")]
        public string EmailPasswordSalt { get; set; }

        [NotMapped, Display(Name = "Email Promo Preference")]
        public string EmailPromotionPreference
        {
            get
            {
                var promoDesc = string.Empty;

                switch (EmailPromotion)
                {
                    case EmailPromoPreference.NoPromotions:
                        promoDesc = "No Promotions";
                        break;

                    case EmailPromoPreference.AdventureWorksOnly:
                        promoDesc = "Adventure-Works Only";
                        break;

                    case EmailPromoPreference.AdventureWorksAndPartners:
                        promoDesc = "Adventure-Works and Partners";
                        break;
                }

                return promoDesc;
            }
        }

        [Required]
        public int ContactTypeID { get; set; }

        [Display(Name = "Position")]
        public string ContactPosition { get; set; }

        public int VendorID { get; set; }

        [NotMapped]
        public IList<PhoneViewModel> Phones { get; set; } = new List<PhoneViewModel>();
    }
}