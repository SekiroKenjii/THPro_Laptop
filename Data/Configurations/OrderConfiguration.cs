using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data.Entities;

namespace Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.AppUser).WithMany(x => x.Orders)
                .HasForeignKey(x => x.CustomerId).OnDelete(DeleteBehavior.Restrict);

            builder.Property(x => x.CustomerName).IsRequired().HasMaxLength(50);
            builder.Property(x => x.EmailAddress).IsRequired().HasMaxLength(100);
            builder.Property(x => x.PhoneNumber).IsRequired().HasMaxLength(20);
            builder.Property(x => x.OrderDateTime).IsRequired();
            builder.Property(x => x.OrderTotalOriginal).IsRequired();
            builder.Property(x => x.OrderTotal).IsRequired();
            builder.Property(x => x.PaymentStatus).IsRequired().HasMaxLength(20);
            builder.Property(x => x.OrderStatus).IsRequired().HasMaxLength(20);
            builder.Property(x => x.TransactionId).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ShipAddress).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ShipCity).IsRequired().HasMaxLength(50);
            builder.Property(x => x.ShipCountry).IsRequired().HasMaxLength(50);
            builder.Property(x => x.CustomerComment).IsRequired().HasMaxLength(256);
        }
    }
}
