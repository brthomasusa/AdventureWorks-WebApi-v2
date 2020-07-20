using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class EmailAddressConfig : IEntityTypeConfiguration<EmailAddress>
    {
        public void Configure(EntityTypeBuilder<EmailAddress> entity)
        {
            entity.HasKey(e => new { e.BusinessEntityID, e.EmailAddressID });
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");
            entity.Property(e => e.EmailAddressID).HasColumnName("EmailAddressID");

            entity.Property(e => e.PersonEmailAddress)
                .IsRequired()
                .HasColumnName("EmailAddress")
                .HasMaxLength(50);

            entity.Property(e => e.RowGuid)
                .HasColumnName("rowguid")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired()
                .HasDefaultValue(Guid.NewGuid());

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValue(DateTime.Now);
        }
    }
}
