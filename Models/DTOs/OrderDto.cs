using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.DTOs
{
    public class OrderDetailsDto
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int Count { get; set; }
    }
    public class CreateOrderDto
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public double OrderTotalOriginal { get; set; }
        public double OrderTotal { get; set; }

        public string ShipAddress { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerComment { get; set; }
        public IList<OrderDetailsDto> OrderDetails { get; set; }
    }

    public class UpdateOrderDto
    {
        [Required]
        public string OrderStatus { get; set; }

        [Required]
        public string PaymentStatus { get; set; }
    }

    public class OrderDto : CreateOrderDto
    {
        public int Id { get; set; }
        public string OrderDateTime { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
    }
}
