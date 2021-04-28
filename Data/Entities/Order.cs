using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }

        public Guid CustomerId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime OrderDateTime { get; set; }
        public double OrderTotalOriginal { get; set; }
        public double OrderTotal { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string TransactionId { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipCountry { get; set; }
        public string CustomerComment { get; set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
