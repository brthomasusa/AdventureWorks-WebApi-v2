using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using AdventureWorks.Models.HumanResources;

namespace AdventureWorks.Dal.EfCode.Configuration.HumanResources
{
    public class JobCandidateConfig : IEntityTypeConfiguration<JobCandidate>
    {
        public void Configure(EntityTypeBuilder<JobCandidate> entity)
        {
            entity.Ignore(e => e.RowGuid);

            entity.HasKey(e => e.JobCandidateID);
            entity.Property(e => e.JobCandidateID).HasColumnName("JobCandidateID");

            entity.Property(e => e.BusinessEntityID)
                .IsRequired(false)
                .HasColumnName("BusinessEntityID");

            entity.Property(e => e.Resume)
                .IsRequired(false)
                .HasColumnName("Resume")
                .HasColumnType("xml");

            entity.Property(e => e.ModifiedDate)
                .HasColumnName("ModifiedDate")
                .IsRequired()
                .HasDefaultValueSql("getdate()");
        }
    }
}