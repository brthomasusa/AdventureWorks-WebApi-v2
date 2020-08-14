using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using AdventureWorks.Models.Base;

namespace AdventureWorks.Models.Person
{
    [Table("Password", Schema = "Person")]
    public class PersonPWord : EntityBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int BusinessEntityID { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Password")]
        public string PasswordHash { get; set; }

        [Required]
        public string PasswordSalt { get; set; }


        [JsonIgnore]
        [ForeignKey(nameof(BusinessEntityID))]
        public virtual Person PersonNavigation { get; set; }
    }
}