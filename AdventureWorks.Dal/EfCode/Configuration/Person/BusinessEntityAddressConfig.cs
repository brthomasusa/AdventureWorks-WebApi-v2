using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    public class BusinessEntityAddressConfig : IEntityTypeConfiguration<BusinessEntityAddress>
    {
        public void Configure(EntityTypeBuilder<BusinessEntityAddress> entity)
        {
            entity.HasKey(e => new { e.BusinessEntityID, e.AddressID, e.AddressTypeID });
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");
            entity.Property(e => e.AddressID).HasColumnName("AddressID");
            entity.Property(e => e.AddressTypeID).HasColumnName("AddressTypeID");

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
