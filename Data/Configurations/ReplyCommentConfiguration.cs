using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    public class ReplyCommentConfiguration : IEntityTypeConfiguration<ReplyUserComment>
    {
        public void Configure(EntityTypeBuilder<ReplyUserComment> builder)
        {
            builder.ToTable("ReplyUserComments");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.UserComment).WithMany(x => x.ReplyUserComments)
                .HasForeignKey(x => x.CommentId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.AppUser).WithMany(x => x.ReplyUserComments)
                .HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.Reply).IsRequired().HasMaxLength(256);
        }
    }
}
