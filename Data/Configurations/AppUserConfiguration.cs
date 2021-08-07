using Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data.Entities;

namespace Data.Configurations
{
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUsers");

            builder.Property(x => x.FirstName).IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.LastName).IsRequired(false).HasMaxLength(50);
            builder.Property(x => x.Address).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.City).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Country).HasMaxLength(20);
            builder.Property(x => x.ProfilePicture).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.PublicId).IsRequired(false).HasMaxLength(200);
            builder.Property(x => x.Gender).HasDefaultValue(Gender.Null);
            builder.Property(x => x.PhoneNumber).IsRequired(false).HasMaxLength(20);
            builder.Property(x => x.NormalizedEmail).HasMaxLength(100);
            builder.Property(x => x.Email).HasMaxLength(100);
            builder.Property(x => x.UserName).HasMaxLength(50);
            builder.Property(x => x.NormalizedUserName).HasMaxLength(50);
        }
    }
}
