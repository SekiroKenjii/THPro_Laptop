using System;

namespace Data.Entities
{
    public class ReplyUserComment
    {
        public int Id { get; set; }

        public int CommentId { get; set; }
        public virtual UserComment UserComment { get; set; }

        public Guid UserId { get; set; }
        public virtual AppUser AppUser { get; set; }

        public string Reply { get; set; }
    }
}
