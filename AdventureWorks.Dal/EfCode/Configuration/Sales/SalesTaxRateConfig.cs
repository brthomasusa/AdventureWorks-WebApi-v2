using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Sales;

namespace AdventureWorks.Dal.EfCode.Configuration.Sales
{
    internal class SalesTaxRateConfig : IEntityTypeConfiguration<SalesTaxRate>
    {
        public void Configure(EntityTypeBuilder<SalesTaxRate> entity)
        {
            entity.HasKey(e => e.SalesTaxRateID);
            entity.Property(e => e.SalesTaxRateID).HasColumnName("SalesTaxRateID");

            entity.Property(e => e.StateProvinceID)
                .IsRequired()
                .HasColumnName("StateProvinceID");

            entity.Property(e => e.TaxType)
                .IsRequired()
                .HasColumnName("TaxType")
                .HasColumnType("tinyint");

            entity.Property(e => e.TaxRate)
                .IsRequired()
                .HasColumnName("TaxRate")
                .HasColumnType("smallmoney");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
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
