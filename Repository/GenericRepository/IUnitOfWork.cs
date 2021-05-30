using Data.Entities;
using System;
using System.Threading.Tasks;

namespace Repository.GenericRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<AppRole> Roles { get; }
        IGenericRepository<Category> Categories { get; }
        IGenericRepository<Condition> Conditions { get; }
        IGenericRepository<Demand> Demands { get; }
        IGenericRepository<Trademark> Trademarks { get; }
        IGenericRepository<Vendor> Vendors { get; }
        IGenericRepository<Product> Products { get; }
        IGenericRepository<ProductImage> ProductImages { get; }
        IGenericRepository<ShoppingCart> ShoppingCarts { get; }
        IGenericRepository<WishList> WishLists { get; }
        //IGenericRepository<Order> Orders { get; }
        //IGenericRepository<OrderDetail> OrderDetails { get; }
        //IGenericRepository<ProductRating> ProductRatings { get; }
        //IGenericRepository<UserComment> UserComments { get; }
        //IGenericRepository<ReplyUserComment> ReplyUserComments { get; }
        Task Save();
    }
}
