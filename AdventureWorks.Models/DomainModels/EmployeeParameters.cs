using System;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.DomainModels
{
    public class EmployeeParameters : QueryStringParameters
    {
        private string[] _activeStatuses = { "Active", "Inactive", "All" };

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string EmployeeStatus { get; set; } = "All";

        public bool IsValidEmployeeStatus
            => Array.Exists(_activeStatuses, element => String.Equals(element, EmployeeStatus, StringComparison.OrdinalIgnoreCase));
    }
}