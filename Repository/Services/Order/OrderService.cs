using AutoMapper;
using Data.EF;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.DTOs;
using Repository.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly DatabaseContext _db;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public OrderService(IUnitOfWork unitOfWork, DatabaseContext db,
            UserManager<AppUser> userManager, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _db = db;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<bool> CreateOrder(CreateOrderDto orderDto)
        {
            var user = await _userManager.FindByIdAsync(orderDto.CustomerId.ToString());

            if(user != null)
            {
                var order = _mapper.Map<Data.Entities.Order>(orderDto);

                if (string.IsNullOrEmpty(orderDto.CustomerName))
                    order.CustomerName = user.FirstName + " " + user.LastName;

                if (string.IsNullOrEmpty(orderDto.EmailAddress))
                    order.EmailAddress = user.Email;

                if (string.IsNullOrEmpty(orderDto.ShipAddress))
                    order.ShipAddress = user.Address + ", " + user.City + ", " + user.Country;

                if (string.IsNullOrEmpty(orderDto.PhoneNumber))
                    order.PhoneNumber = user.PhoneNumber;

                order.OrderDateTime = DateTime.Now;
                order.OrderStatus = "Chờ xác nhận";
                order.PaymentStatus = "Đang chờ";

                await _unitOfWork.Orders.Insert(order);
                await _unitOfWork.Save();

                foreach (var details in orderDto.OrderDetails)
                {
                    var currentCart = await _unitOfWork.ShoppingCarts.Get(x => x.UserId == user.Id &&
                        x.ProductId == details.ProductId);

                    await _unitOfWork.ShoppingCarts.Delete(currentCart.Id);

                    var product = await _unitOfWork.Products.Get(x => x.Id == details.ProductId);

                    product.UnitsInStock -= details.Count;
                    product.UnitsOnOrder += details.Count;

                    _unitOfWork.Products.Update(product);

                    await _unitOfWork.Save();
                }

                return true;
            }

            return false;
        }

        public async Task<IList<OrderDto>> GetAllOrders()
        {
            var orders = await _db.Orders.Include(x => x.OrderDetails).ToListAsync();
            var result = _mapper.Map<IList<OrderDto>>(orders);
            return result;
        }

        public async Task<IList<OrderDto>> GetOrderDetails(Guid CustomerId)
        {
            var user = await _userManager.FindByIdAsync(CustomerId.ToString());

            if (user != null)
            {
                var orders = await _db.Orders.Include(x=>x.OrderDetails).Where(x => x.CustomerId == user.Id).ToListAsync();
                var result = _mapper.Map<IList<OrderDto>>(orders);
                return result;
            }

            return null;
        }

        public async Task<bool> UpdateOrder(int OrderId, UpdateOrderDto updateOrderDto)
        {
            var order = await _db.Orders.FirstOrDefaultAsync(x => x.Id == OrderId);

            if (order != null)
            {
                order.OrderStatus = updateOrderDto.OrderStatus;
                order.PaymentStatus = updateOrderDto.PaymentStatus;

                _unitOfWork.Orders.Update(order);
                await _unitOfWork.Save();
                return true;
            }

            return false;
        }
    }
}
