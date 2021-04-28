using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class UserCommentConfiguration : IEntityTypeConfiguration<UserComment>
    {
        public void Configure(EntityTypeBuilder<UserComment> builder)
        {
            builder.ToTable("UserComments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.AppUser).WithMany(x => x.UserComments)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Product).WithMany(x => x.UserComments)
                .HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Comment).IsRequired().HasMaxLength(256);
        }
    }
}
