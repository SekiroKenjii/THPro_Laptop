using System;
using System.Collections.Generic;

namespace Data.Entities
{
    public class UserComment
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public Guid UserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public string Comment { get; set; }

        public virtual IEnumerable<ReplyUserComment> ReplyUserComments { get; set; }
    }
}
