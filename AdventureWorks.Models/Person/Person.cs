using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;
using AdventureWorks.Models.HumanResources;


namespace AdventureWorks.Models.Person
{
    [Table("Person", Schema = "Person")]
    public class Person : PersonBase
    {
        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual BusinessEntity BusinessEntityNavigation { get; set; }

        [InverseProperty(nameof(EmailAddress.PersonNavigation))]
        public virtual EmailAddress EmailAddressObj { get; set; }

        [InverseProperty(nameof(PersonPhone.PersonNavigation))]
        public virtual IList<PersonPhone> Phones { get; set; } = new List<PersonPhone>();

        [InverseProperty(nameof(PersonPWord.PersonNavigation))]
        public virtual PersonPWord PasswordObj { get; set; }

        [InverseProperty(nameof(Employee.PersonNavigation))]
        public virtual Employee EmployeeObj { get; set; }
    }
}