using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class BusinessEntityContactConfig : IEntityTypeConfiguration<BusinessEntityContact>
    {
        public void Configure(EntityTypeBuilder<BusinessEntityContact> entity)
        {
            entity.HasKey(e => new { e.BusinessEntityID, e.PersonID, e.ContactTypeID });
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");
            entity.Property(e => e.PersonID).HasColumnName("PersonID");
            entity.Property(e => e.ContactTypeID).HasColumnName("ContactTypeID");

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
