using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Dal.EfCode.Configuration.HumanResources
{
    internal class DepartmentConfig : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => e.DepartmentID);
            entity.Property(e => e.DepartmentID).HasColumnName("DepartmentID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(50);

            entity.Property(e => e.GroupName)
                .IsRequired()
                .HasColumnName("GroupName")
                .HasMaxLength(50);

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValueSql("getdate()");
        }
    }
}