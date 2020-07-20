using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Dal.EfCode.Configuration.HumanResources
{
    public class ShiftConfig : IEntityTypeConfiguration<Shift>
    {
        public void Configure(EntityTypeBuilder<Shift> entity)
        {

            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => e.ShiftID);
            entity.Property(e => e.ShiftID).HasColumnName("ShiftID");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasMaxLength(50);

            entity.Property(e => e.StartTime)
                .IsRequired()
                .HasColumnName("StartTime")
                .HasColumnType("time(7)");

            entity.Property(e => e.EndTime)
                .IsRequired()
                .HasColumnName("EndTime")
                .HasColumnType("time(7)");

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValueSql("getdate()");
        }
    }
}