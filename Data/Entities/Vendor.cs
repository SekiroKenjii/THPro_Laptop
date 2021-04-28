﻿using Data.Enums;
using System.Collections.Generic;

namespace Data.Entities
{
    public class Vendor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string HomePage { get; set; }
        public Status Status { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
