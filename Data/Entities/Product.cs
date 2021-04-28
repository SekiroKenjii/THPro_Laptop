﻿using System.Collections.Generic;

namespace Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int VendorId { get; set; }
        public virtual Vendor Vendor { get; set; }

        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public int ConditionId { get; set; }
        public virtual Condition Condition { get; set; }

        public int DemandId { get; set; }
        public virtual Demand Demand { get; set; }

        public int TrademarkId { get; set; }
        public virtual Trademark Trademark { get; set; }

        public string Cpu { get; set; }
        public string Screen { get; set; }
        public string Ram { get; set; }
        public string Gpu { get; set; }
        public string Storage { get; set; }
        public string Pin { get; set; }
        public string Connection { get; set; }
        public string Weight { get; set; }
        public string OS { get; set; }
        public string Color { get; set; }
        public string Warranty { get; set; }
        public double Price { get; set; }
        public int UnitsInStock { get; set; }
        public int UnitsOnOrder { get; set; }
        public string Description { get; set; }
        public bool Discontinued { get; set; }

        public virtual IEnumerable<OrderDetail> OrderDetails { get; set; }
        public virtual IEnumerable<ProductImage> ProductImages { get; set; }
        public virtual IEnumerable<WishList> WishLists { get; set; }
        public virtual IEnumerable<ShoppingCart> ShoppingCarts { get; set; }
        public virtual IEnumerable<UserComment> UserComments { get; set; }
        public virtual IEnumerable<ProductRating> ProductRatings { get; set; }
    }
}
