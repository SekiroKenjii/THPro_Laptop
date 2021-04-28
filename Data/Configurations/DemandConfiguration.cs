using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data.Entities;

namespace Data.Configurations
{
    public class DemandConfiguration : IEntityTypeConfiguration<Demand>
    {
        public void Configure(EntityTypeBuilder<Demand> builder)
        {
            builder.ToTable("Demands");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
        }
    }
}
