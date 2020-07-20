using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Dal.EfCode.Configuration.HumanResources
{
    public class EmployeeDepartmentHistoryConfig : IEntityTypeConfiguration<EmployeeDepartmentHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeeDepartmentHistory> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => new { e.BusinessEntityID, e.DepartmentID, e.ShiftID, e.StartDate });
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");

            entity.Property(e => e.DepartmentID)
                .IsRequired()
                .HasColumnName("DepartmentID")
                .HasColumnType("smallint");

            entity.Property(e => e.ShiftID)
                .IsRequired()
                .HasColumnName("ShiftID")
                .HasColumnType("tinyint");

            entity.Property(e => e.StartDate)
                .IsRequired()
                .HasColumnName("StartDate")
                .HasColumnType("date");

            entity.Property(e => e.EndDate)
                .HasColumnName("EndDate")
                .HasColumnType("date");

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValueSql("getdate()");
        }
    }
}