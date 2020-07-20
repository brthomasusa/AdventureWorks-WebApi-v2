using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class CountryRegionConfig : IEntityTypeConfiguration<CountryRegion>
    {
        public void Configure(EntityTypeBuilder<CountryRegion> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => e.CountryRegionCode);
            entity.Property(e => e.CountryRegionCode).HasColumnName("CountryRegionCode");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(50);

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValue(DateTime.Now);
        }
    }
}
