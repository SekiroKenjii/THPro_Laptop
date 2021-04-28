using System.Collections.Generic;

namespace Data.Entities
{
    public class Demand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IEnumerable<Product> Products { get; set; }
    }
}
