using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AdventureWorks.Mvc.Views.Shared.LookupData
{
    public class CreditRatingLookupCollection
    {
        public static List<SelectListItem> CreditRatingStatuses()
        {
            return new List<SelectListItem> {
                new SelectListItem { Value = "0", Text = "-- Not Selected --"},
                new SelectListItem { Value = "1", Text = "Superior"},
                new SelectListItem { Value = "2", Text = "Excellent"},
                new SelectListItem { Value = "3", Text = "Above Average"},
                new SelectListItem { Value = "4", Text = "Average"},
                new SelectListItem { Value = "5", Text = "Below Average"}
            };
        }
    }
}