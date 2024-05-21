using Domain.Entities;

namespace backend.version1.Domain.Entities
{
    public class Reaction : BaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public bool IsLike { get; set; } = false;

        public ICollection<PostReaction> PostReactions { get; set; } = new List<PostReaction>();
    }
}