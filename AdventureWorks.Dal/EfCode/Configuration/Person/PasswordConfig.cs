using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.Person;

namespace AdventureWorks.Dal.EfCode.Configuration.Person
{
    internal class PasswordConfig : IEntityTypeConfiguration<PersonPWord>
    {
        public void Configure(EntityTypeBuilder<PersonPWord> entity)
        {
            entity.HasKey(e => e.BusinessEntityID);
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");

            entity.Property(e => e.PasswordHash)
                .IsRequired()
                .HasColumnName("PasswordHash")
                .HasMaxLength(128);

            entity.Property(e => e.PasswordSalt)
                .IsRequired()
                .HasColumnName("PasswordSalt")
                .HasMaxLength(10);

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
