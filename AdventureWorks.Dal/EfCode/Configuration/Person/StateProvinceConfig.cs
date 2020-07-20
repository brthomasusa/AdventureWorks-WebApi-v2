using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class StateProvinceConfig : IEntityTypeConfiguration<StateProvince>
    {
        public void Configure(EntityTypeBuilder<StateProvince> entity)
        {
            entity.HasKey(e => e.StateProvinceID);
            entity.Property(e => e.StateProvinceID).HasColumnName("StateProvinceID");

            entity.Property(e => e.StateProvinceCode)
                .IsRequired()
                .HasColumnName("StateProvinceCode")
                .HasColumnType("nchar(3)");

            entity.Property(e => e.CountryRegionCode)
                .IsRequired()
                .HasColumnName("CountryRegionCode")
                .HasMaxLength(3);

            entity.Property(e => e.IsOnlyStateProvince)
                .IsRequired()
                .HasColumnName("IsOnlyStateProvinceFlag")
                .HasColumnType("bit");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(50);

            entity.Property(e => e.TerritoryID)
                .IsRequired()
                .HasColumnName("TerritoryID");

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
