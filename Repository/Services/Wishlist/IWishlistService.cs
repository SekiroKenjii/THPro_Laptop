using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.Wishlist
{
    public interface IWishlistService
    {
        Task<IList<WishlistDto>> GetWishlistCount(Guid userId);
        Task<bool> AddToWishlist(CreateWishlistDto wishlistDto);
        Task<bool> RemoveWishlistItem(int id);
    }
}
