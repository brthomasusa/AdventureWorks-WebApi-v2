using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class PersonPhoneConfig : IEntityTypeConfiguration<PersonPhone>
    {
        public void Configure(EntityTypeBuilder<PersonPhone> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => new { e.BusinessEntityID, e.PhoneNumber, e.PhoneNumberTypeID });
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");
            entity.Property(e => e.PhoneNumber).HasColumnName("PhoneNumber").HasColumnType("dbo.Phone");
            entity.Property(e => e.PhoneNumberTypeID).HasColumnName("PhoneNumberTypeID");

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValue(DateTime.Now);
        }
    }
}
