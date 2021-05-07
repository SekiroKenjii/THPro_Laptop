using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Model.DTOs;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.Cart
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public CartService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<bool> AddToCart(CreateCartDto cartDto)
        {
            var product = await _unitOfWork.Products.Get(x => x.Id == cartDto.ProductId);
            var user = await _userManager.FindByIdAsync(cartDto.UserId.ToString());

            if (product != null && product.UnitsInStock >= cartDto.Count && user != null)
            {
                var cart = await _unitOfWork.ShoppingCarts.Get(x => x.UserId == user.Id && x.ProductId == product.Id);

                if (cart == null)
                {
                    await _unitOfWork.ShoppingCarts.Insert(new ShoppingCart()
                    {
                        UserId = user.Id,
                        ProductId = product.Id,
                        Count = cartDto.Count
                    });

                    await _unitOfWork.Save();
                    return true;
                }

                cart.Count += cartDto.Count;
                _unitOfWork.ShoppingCarts.Update(cart);
                await _unitOfWork.Save();
                return true;
            }

            return false;
        }

        public async Task<IList<CartDto>> GetCartCount(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception("Submitted data is invalid");

            IList<CartDto> result = new List<CartDto>();
            var carts = await _unitOfWork.ShoppingCarts.GetAll(x => x.UserId == userId);
            
            foreach(var cart in carts)
            {
                var product = await _unitOfWork.Products.Get(x => x.Id == cart.ProductId,
                    new List<string> { "Category", "Condition", "Demand", "Trademark", "Vendor", "ProductImages" });
                cart.Product = product;
                result.Add(_mapper.Map<CartDto>(cart));
            }
            return result;
        }

        public async Task<bool> RemoveCartItem(int id)
        {
            var cart = await _unitOfWork.ShoppingCarts.Get(x => x.Id == id);

            if (cart == null)
                return false;

            await _unitOfWork.ShoppingCarts.Delete(cart.Id);
            await _unitOfWork.Save();
            return true;
        }

        public async Task<bool> UpdateCart(Guid userId, IList<UpdateCartDto> cartsDto)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null || cartsDto.Count == 0)
                return false;

            foreach (var cartDto in cartsDto)
            {
                var product = await _unitOfWork.Products.Get(x => x.Id == cartDto.ProductId);
                var cart = await _unitOfWork.ShoppingCarts.Get(x => x.UserId == user.Id && x.ProductId == cartDto.ProductId);

                if (product == null || product.UnitsInStock < cartDto.Count || cart == null)
                    return false;

                if (cartDto.Count != cart.Count)
                {
                    cart.Count = cartDto.Count;
                    _unitOfWork.ShoppingCarts.Update(cart);
                    await _unitOfWork.Save();
                }
            }
            return true;
        }
    }
}
