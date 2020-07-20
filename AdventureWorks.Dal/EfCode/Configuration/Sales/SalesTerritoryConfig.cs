using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Sales;

namespace AdventureWorks.Dal.EfCode.Configuration.Sales
{
    internal class SalesTerritoryConfig : IEntityTypeConfiguration<SalesTerritory>
    {
        public void Configure(EntityTypeBuilder<SalesTerritory> entity)
        {
            entity.HasKey(e => e.TerritoryID);
            entity.Property(e => e.TerritoryID).HasColumnName("TerritoryID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(50);

            entity.Property(e => e.CountryRegionCode)
                .IsRequired()
                .HasColumnName("CountryRegionCode");

            entity.Property(e => e.Group)
                .IsRequired()
                .HasColumnName("Group")
                .HasMaxLength(50);

            entity.Property(e => e.SalesYTD)
                .IsRequired()
                .HasColumnName("SalesYTD")
                .HasColumnType("money");

            entity.Property(e => e.SalesLastYear)
                .IsRequired()
                .HasColumnName("SalesLastYear")
                .HasColumnType("money");

            entity.Property(e => e.CostLastYear)
                .IsRequired()
                .HasColumnName("CostLastYear")
                .HasColumnType("money");

            entity.Property(e => e.CostYTD)
                .IsRequired()
                .HasColumnName("CostYTD")
                .HasColumnType("money");

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
