using System;

namespace Data.Entities
{
    public class WishList
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
