using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class ContactTypeConfig : IEntityTypeConfiguration<ContactType>
    {
        public void Configure(EntityTypeBuilder<ContactType> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => e.ContactTypeID);
            entity.Property(e => e.ContactTypeID).HasColumnName("ContactTypeID");

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
