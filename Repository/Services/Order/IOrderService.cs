using Model.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository.Services.Order
{
    public interface IOrderService
    {
        Task<bool> CreateOrder(CreateOrderDto orderDto);
        Task<IList<OrderDto>> GetOrderDetails(Guid CustomerId);
        Task<IList<OrderDto>> GetAllOrders();
        Task<bool> UpdateOrder(int OrderId, UpdateOrderDto updateOrderDto);
    }
}
