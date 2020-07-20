using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class AddressConfig : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> entity)
        {
            entity.HasKey(e => e.AddressID);
            entity.Property(e => e.AddressID).HasColumnName("AddressID");

            entity.Property(e => e.AddressLine1)
                .IsRequired()
                .HasColumnName("AddressLine1")
                .HasMaxLength(60);

            entity.Property(e => e.AddressLine2)
                .IsRequired(false)
                .HasColumnName("AddressLine2")
                .HasMaxLength(60);

            entity.Property(e => e.City)
                .IsRequired()
                .HasColumnName("City")
                .HasMaxLength(30);

            entity.Property(e => e.StateProvinceID)
                .IsRequired()
                .HasColumnName("StateProvinceID");

            entity.Property(e => e.PostalCode)
                .IsRequired()
                .HasColumnName("PostalCode")
                .HasMaxLength(60);

            entity.Property(e => e.SpatialLocation)
                .IsRequired(false)
                .HasColumnName("SpatialLocation")
                .HasColumnType("geography");

            entity.Property(e => e.RowGuid)
                .HasColumnName("rowguid")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired()
                .HasDefaultValue(Guid.NewGuid());

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValueSql("getdate()");
        }
    }
}
