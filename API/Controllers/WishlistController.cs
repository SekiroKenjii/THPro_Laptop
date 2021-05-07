using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.Services.Wishlist;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        private readonly ILogger<WishlistController> _logger;
        public WishlistController(IWishlistService wishlistService, ILogger<WishlistController> logger)
        {
            _wishlistService = wishlistService;
            _logger = logger;
        }

        [HttpGet("api/wishlist/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetWishlistItem(Guid userId)
        {
            var wishList = await _wishlistService.GetWishlistCount(userId);
            return Ok(wishList);
        }

        [HttpPost("api/wishlist/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddToWishlist([FromBody] CreateWishlistDto wishlistDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddToWishlist)}");
                return BadRequest(ModelState);
            }
            var result = await _wishlistService.AddToWishlist(wishlistDto);
            if (!result)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddToWishlist)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }

        [HttpDelete("api/wishlist/delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveWishlistItem(int id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(RemoveWishlistItem)}");
                return BadRequest(ModelState);
            }
            var result = await _wishlistService.RemoveWishlistItem(id);
            if (!result)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(RemoveWishlistItem)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }
    }
}
