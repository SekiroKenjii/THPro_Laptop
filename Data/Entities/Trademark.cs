using System.Collections.Generic;
using Data.Enums;

namespace Data.Entities
{
    public class Trademark
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public string Description { get; set; }
        public Status Status { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
