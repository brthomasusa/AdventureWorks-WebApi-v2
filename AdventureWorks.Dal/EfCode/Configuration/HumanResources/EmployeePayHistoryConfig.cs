using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Dal.EfCode.Configuration.HumanResources
{
    public class EmployeePayHistoryConfig : IEntityTypeConfiguration<EmployeePayHistory>
    {
        public void Configure(EntityTypeBuilder<EmployeePayHistory> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => new { e.BusinessEntityID, e.RateChangeDate });
            entity.Property(e => e.BusinessEntityID).HasColumnName("BusinessEntityID");

            entity.Property(e => e.RateChangeDate)
                .IsRequired()
                .HasColumnName("RateChangeDate")
                .HasColumnType("datetime");

            entity.Property(e => e.Rate)
                .IsRequired()
                .HasColumnName("Rate")
                .HasColumnType("money");

            entity.Property(e => e.PayFrequency)
                .IsRequired()
                .HasColumnName("PayFrequency")
                .HasColumnType("tinyint");

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValueSql("getdate()");
        }
    }
}