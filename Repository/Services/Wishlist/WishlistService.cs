using AutoMapper;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Model.DTOs;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.Wishlist
{
    public class WishlistService : IWishlistService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WishlistService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<bool> AddToWishlist(CreateWishlistDto wishlistDto)
        {
            var product = await _unitOfWork.Products.Get(x => x.Id == wishlistDto.ProductId);
            var user = await _userManager.FindByIdAsync(wishlistDto.UserId.ToString());

            if (product != null && user != null)
            {
                var wishList = await _unitOfWork.WishLists.Get(x => x.UserId == user.Id && x.ProductId == product.Id);

                if(wishList == null)
                {
                    await _unitOfWork.WishLists.Insert(new WishList()
                    {
                        UserId = user.Id,
                        ProductId = product.Id
                    });

                    await _unitOfWork.Save();
                    return true;
                }
                return false;
            }

            return false;
        }

        public async Task<IList<WishlistDto>> GetWishlistCount(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                throw new Exception("Submitted data is invalid");

            IList<WishlistDto> result = new List<WishlistDto>();
            var wishLists = await _unitOfWork.WishLists.GetAll(x => x.UserId == user.Id);

            foreach(var wishList in wishLists)
            {
                var product = await _unitOfWork.Products.Get(x => x.Id == wishList.ProductId,
                    new List<string> { "Category", "Condition", "Demand", "Trademark", "Vendor", "ProductImages" });
                wishList.Product = product;
                result.Add(_mapper.Map<WishlistDto>(wishList));
            }

            return result;
        }

        public async Task<bool> RemoveWishlistItem(int id)
        {
            var wishList = await _unitOfWork.WishLists.Get(x => x.Id == id);

            if (wishList != null)
            {
                await _unitOfWork.WishLists.Delete(wishList.Id);
                await _unitOfWork.Save();
                return true;
            }
            return false;
        }
    }
}
