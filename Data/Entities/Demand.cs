using System.Collections.Generic;

namespace Data.Entities
{
    public class Demand
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
