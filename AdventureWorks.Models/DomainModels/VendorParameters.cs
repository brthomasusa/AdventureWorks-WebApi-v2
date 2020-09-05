using System;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.DomainModels
{
    public class VendorParameters : QueryStringParameters
    {
        private string[] _activeStatuses = { "Active", "Inactive", "All" };

        public string Name { get; set; }

        public string VendorStatus { get; set; } = "All";

        public bool IsValidVendorStatus
            => Array.Exists(_activeStatuses, element => String.Equals(element, VendorStatus, StringComparison.OrdinalIgnoreCase));
    }
}