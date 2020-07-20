using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Dal.EfCode.Configuration.HumanResources
{
    public class EmployeeConfig : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> entity)
        {
            entity.HasKey(e => e.BusinessEntityID);
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");

            entity.Property(e => e.NationalIDNumber)
                .IsRequired()
                .HasColumnName("NationalIDNumber")
                .HasMaxLength(15);

            entity.Property(e => e.LoginID)
                .IsRequired()
                .HasColumnName("LoginID")
                .HasMaxLength(256);

            entity.Property(e => e.JobTitle)
                .IsRequired()
                .HasColumnName("JobTitle")
                .HasMaxLength(50);

            entity.Property(e => e.BirthDate)
                .IsRequired()
                .HasColumnName("BirthDate")
                .HasColumnType("date");

            entity.Property(e => e.MaritalStatus)
                .IsRequired()
                .HasColumnName("MaritalStatus")
                .HasColumnType("nchar(1)");

            entity.Property(e => e.Gender)
                .IsRequired()
                .HasColumnName("Gender")
                .HasColumnType("nchar(1)");

            entity.Property(e => e.HireDate)
                .IsRequired()
                .HasColumnName("HireDate")
                .HasColumnType("date");

            entity.Property(e => e.IsSalaried)
                .IsRequired()
                .HasColumnName("SalariedFlag")
                .HasColumnType("bit");

            entity.Property(e => e.VacationHours)
                .IsRequired()
                .HasColumnName("VacationHours")
                .HasColumnType("smallint");

            entity.Property(e => e.SickLeaveHours)
                .IsRequired()
                .HasColumnName("SickLeaveHours")
                .HasColumnType("smallint");

            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasColumnName("CurrentFlag")
                .HasColumnType("bit");

            entity.Property(e => e.RowGuid)
                .HasColumnName("rowguid")
                .HasColumnType("UNIQUEIDENTIFIER")
                .IsRequired()
                .HasDefaultValueSql("newid()");

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValueSql("getdate()");
        }

    }
}