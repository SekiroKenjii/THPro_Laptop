using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.Services.Cart;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;
        public CartController(ICartService cartService, ILogger<CartController> logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpGet("api/cart/{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCartItem(Guid userId)
        {
            var carts = await _cartService.GetCartCount(userId);
            return Ok(carts);
        }

        [HttpPost("api/cart/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddToCart([FromBody] CreateCartDto cartDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddToCart)}");
                return BadRequest(ModelState);
            }
            var result = await _cartService.AddToCart(cartDto);
            if (!result)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddToCart)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }

        [HttpPut("api/cart/update/{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCart(Guid userId, [FromBody] IList<UpdateCartDto> cartsDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCart)}");
                return BadRequest(ModelState);
            }
            var result = await _cartService.UpdateCart(userId, cartsDto);
            if (!result)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCart)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }

        [HttpDelete("api/cart/delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveCartItem(int id)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(RemoveCartItem)}");
                return BadRequest(ModelState);
            }
            var result = await _cartService.RemoveCartItem(id);
            if (!result)
            {
                _logger.LogError($"Invalid DELETE attempt in {nameof(RemoveCartItem)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }
    }
}
