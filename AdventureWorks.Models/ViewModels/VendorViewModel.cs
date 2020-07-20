using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdventureWorks.Models.CustomTypes;
using AdventureWorks.Models.Base.Purchasing;

namespace AdventureWorks.Models.ViewModel
{    
    public class VendorViewModel : VendorBase
    {  
        [NotMapped, Display(Name="Credit Rating")]
        public string CreditRatingText 
        { 
            get {
                
                var retVal = string.Empty;

                if (CreditRating > 0) 
                {
                    switch (CreditRating)
                    {
                        case CreditRating.Superior:
                            retVal = "Superior";
                            break;
                        case CreditRating.Excellent:
                            retVal = "Excellent";
                            break;
                        case CreditRating.Above_Average:
                            retVal = "Above Average";
                            break;
                        case CreditRating.Average:
                            retVal = "Average";
                            break;
                        case CreditRating.Below_Average:
                            retVal = "Below Average";
                            break;
                    }
                    return retVal;
                }

                return "Not Set!";
            } 
        }

        [NotMapped]
        public IList<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();

        [NotMapped]
        public IList<VendorContactViewModel> Contacts { get; set; } = new List<VendorContactViewModel>();
    }
}