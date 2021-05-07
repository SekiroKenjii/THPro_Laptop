using Data.EF;
using Data.Entities;
using System;
using System.Threading.Tasks;

namespace Repository.GenericRepository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IGenericRepository<Category> _categories;
        private IGenericRepository<Condition> _conditions;
        private IGenericRepository<Demand> _demands;
        private IGenericRepository<Trademark> _trademarks;
        private IGenericRepository<Vendor> _vendors;
        private IGenericRepository<Product> _products;
        private IGenericRepository<ProductImage> _productImages;
        private IGenericRepository<ShoppingCart> _carts;
        private IGenericRepository<WishList> _wishlist;
        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public IGenericRepository<Category> Categories => _categories ??= new GenericRepository<Category>(_context);

        public IGenericRepository<Condition> Conditions => _conditions ??= new GenericRepository<Condition>(_context);

        public IGenericRepository<Demand> Demands => _demands ??= new GenericRepository<Demand>(_context);

        public IGenericRepository<Trademark> Trademarks => _trademarks ??= new GenericRepository<Trademark>(_context);

        public IGenericRepository<Vendor> Vendors => _vendors ??= new GenericRepository<Vendor>(_context);

        public IGenericRepository<Product> Products => _products ??= new GenericRepository<Product>(_context);

        public IGenericRepository<ProductImage> ProductImages => _productImages ??= new GenericRepository<ProductImage>(_context);

        public IGenericRepository<ShoppingCart> ShoppingCarts => _carts ??= new GenericRepository<ShoppingCart>(_context);

        public IGenericRepository<WishList> WishLists => _wishlist ??= new GenericRepository<WishList>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
