using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.Cart
{
    public interface ICartService
    {
        Task<IList<CartDto>> GetCartCount(Guid userId);
        Task<bool> AddToCart(CreateCartDto cartDto);
        Task<bool> UpdateCart(Guid userId, IList<UpdateCartDto> cartsDto);
        Task<bool> RemoveCartItem(int id);
    }
}
