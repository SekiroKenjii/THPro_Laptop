using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Data.Entities;

namespace Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn();

            builder.HasOne(x => x.Vendor).WithMany(x => x.Products)
                .HasForeignKey(x => x.VendorId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Category).WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Condition).WithMany(x => x.Products)
                .HasForeignKey(x => x.ConditionId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Demand).WithMany(x => x.Products)
                .HasForeignKey(x => x.DemandId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(x => x.Trademark).WithMany(x => x.Products)
                .HasForeignKey(x => x.TrademarkId).OnDelete(DeleteBehavior.Restrict);
            

            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Cpu).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Screen).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Ram).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Gpu).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Storage).IsRequired().HasMaxLength(50);
            builder.Property(x => x.Pin).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Connection).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Weight).IsRequired().HasMaxLength(10);
            builder.Property(x => x.OS).IsRequired().HasMaxLength(20);
            builder.Property(x => x.Color).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Warranty).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Price).IsRequired().HasDefaultValue(0.0);
            builder.Property(x => x.UnitsInStock).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.UnitsOnOrder).IsRequired().HasDefaultValue(0);
            builder.Property(x => x.Discontinued).IsRequired();
        }
    }
}
