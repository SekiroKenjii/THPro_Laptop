﻿using System.Collections.Generic;

namespace Data.Entities
{
    public class Condition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
