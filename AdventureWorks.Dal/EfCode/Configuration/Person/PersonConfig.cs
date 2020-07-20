using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class PersonConfig : IEntityTypeConfiguration<AdventureWorks.Models.Person.Person>
    {
        public void Configure(EntityTypeBuilder<AdventureWorks.Models.Person.Person> entity)
        {
            entity.HasKey(e => e.BusinessEntityID);
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");

            entity.Property(e => e.PersonType)
                .IsRequired()
                .HasColumnName("PersonType")
                .HasMaxLength(2);

            entity.Property(e => e.IsEasternNameStyle)
                .IsRequired()
                .HasColumnName("NameStyle")
                .HasColumnType("dbo.NameStyle");

            entity.Property(e => e.Title)
                .IsRequired(false)
                .HasColumnName("Title")
                .HasMaxLength(8);

            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasColumnName("FirstName")
                .HasMaxLength(50);

            entity.Property(e => e.MiddleName)
                .IsRequired(false)
                .HasColumnName("MiddleName")
                .HasMaxLength(50);

            entity.Property(e => e.LastName)
                .IsRequired()
                .HasColumnName("LastName")
                .HasMaxLength(50);

            entity.Property(e => e.Suffix)
                .IsRequired(false)
                .HasColumnName("Suffix")
                .HasMaxLength(10);

            entity.Property(e => e.EmailPromotion)
                .IsRequired()
                .HasColumnName("EmailPromotion");

            entity.Property(e => e.AdditionalContactInfo)
                .IsRequired(false)
                .HasColumnName("AdditionalContactInfo")
                .HasColumnType("xml");

            entity.Property(e => e.Demographics)
                .IsRequired(false)
                .HasColumnName("Demographics")
                .HasColumnType("xml");

            entity.Property(e => e.RowGuid)
                .IsRequired()
                .HasColumnName("rowguid")
                .HasColumnType("UNIQUEIDENTIFIER")
                .HasDefaultValue(Guid.NewGuid());

            entity.Property(e => e.ModifiedDate)
                .IsRequired()
                .HasColumnName("ModifiedDate")
                .HasDefaultValueSql("getdate()");
        }
    }
}
