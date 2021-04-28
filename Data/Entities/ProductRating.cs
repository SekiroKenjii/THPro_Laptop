using System;

namespace Data.Entities
{
    public class ProductRating
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public Guid UserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public float RatingLevel { get; set; }
        public string Comment { get; set; }
    }
}
