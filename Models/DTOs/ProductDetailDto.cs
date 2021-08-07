using Data.Entities;
using System.Collections.Generic;

namespace Model.DTOs
{
    public class ProductDetailDto
    {
        public IList<Category> Categories { get; set; }
        public IList<Vendor> Vendors { get; set; }
        public IList<Trademark> Trademarks { get; set; }
        public IList<Demand> Demands { get; set; }
        public IList<Condition> Conditions { get; set; }
    }
}
