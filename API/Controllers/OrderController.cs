using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.DTOs;
using Repository.Services.Order;
using System;
using System.Threading.Tasks;
using Utility.Extensions;

namespace API.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [Authorize(Roles = ApplicationStaticExtensions.AdminRole + "," + 
            ApplicationStaticExtensions.WarehouseRole + "," + ApplicationStaticExtensions.CustomerOfficerRole)]
        [HttpGet("api/orders")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllOrder()
        {
            var result = await _orderService.GetAllOrders();
            return Ok(result);
        }

        [HttpGet("api/orders/{customerId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderDetails(Guid customerId)
        {
            var result = await _orderService.GetOrderDetails(customerId);
            return Ok(result);
        }

        [HttpPost("api/order/add")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddOrder)}");
                return BadRequest(ModelState);
            }
            var result = await _orderService.CreateOrder(orderDto);
            if (!result)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(AddOrder)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }

        [HttpPut("api/order/update/{orderId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateOrder(int orderId, [FromBody] UpdateOrderDto updateOrderDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateOrder)}");
                return BadRequest(ModelState);
            }
            var result = await _orderService.UpdateOrder(orderId, updateOrderDto);
            if (!result)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateOrder)}");
                return BadRequest("Submitted data is invalid");
            }
            return NoContent();
        }
    }
}
