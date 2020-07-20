using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Purchasing;

namespace AdventureWorks.Dal.EfCode.Configuration.Purchasing
{
    internal class VendorConfig : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => e.BusinessEntityID);
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");

            entity.Property(e => e.AccountNumber)
                .IsRequired()
                .HasColumnName("AccountNumber")
                .HasMaxLength(15);

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(50);

            entity.Property(e => e.CreditRating)
                .IsRequired()
                .HasColumnName("CreditRating")
                .HasColumnType("tinyint");

            entity.Property(e => e.PreferredVendor)
                .IsRequired()
                .HasColumnName("PreferredVendorStatus")
                .HasColumnType("bit");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnName("ActiveFlag")
                .HasColumnType("bit");

            entity.Property(e => e.PurchasingWebServiceURL)
                .IsRequired(false)
                .HasColumnName("PurchasingWebServiceURL")
                .HasMaxLength(1024);

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValue(DateTime.Now);
        }
    }
}
