using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data.Entities;

namespace Data.Configurations
{
    public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
    {
        public void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.ToTable("Vendors");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ContactName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ContactTitle).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Address).IsRequired().HasMaxLength(100);
            builder.Property(x => x.City).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Country).IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(x => x.HomePage).IsUnicode(false).IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.Status).IsRequired().HasDefaultValue(Status.InActive);
        }
    }
}
