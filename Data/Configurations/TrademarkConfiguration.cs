using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data.Entities;

namespace Data.Configurations
{
    class TrademarkConfiguration : IEntityTypeConfiguration<Trademark>
    {
        public void Configure(EntityTypeBuilder<Trademark> builder)
        {
            builder.ToTable("Trademarks");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(20);
            builder.Property(x => x.ImageUrl).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired(false).HasMaxLength(256);
            builder.Property(x => x.Status).IsRequired().HasDefaultValue(Status.InActive);
            builder.Property(x => x.PublicId).IsRequired().HasMaxLength(200);
        }
    }
}
