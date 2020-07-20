using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class PhoneNumberTypeConfig : IEntityTypeConfiguration<PhoneNumberType>
    {
        public void Configure(EntityTypeBuilder<PhoneNumberType> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => e.PhoneNumberTypeID);
            entity.Property(e => e.PhoneNumberTypeID).HasColumnName("PhoneNumberTypeID");

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
