using Data.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class AppUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ProfilePicture { get; set; }
        public string PublicId { get; set; }
        public Gender Gender { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public virtual IEnumerable<Order> Orders { get; set; }
        public virtual IEnumerable<UserComment> UserComments { get; set; }
        public virtual IEnumerable<ReplyUserComment> ReplyUserComments { get; set; }
        public virtual IEnumerable<ShoppingCart> ShoppingCarts { get; set; }
        public virtual IEnumerable<WishList> WishLists { get; set; }
        public virtual IEnumerable<ProductRating> ProductRatings { get; set; }
    }
}
