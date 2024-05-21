using Domain.Entities;

namespace backend.version1.Domain.Entities
{
    public class PostReaction : BaseEntity
    {
        public int PostId { get; set; }
        public int ReactionId { get; set; }
        public Post Post { get; set; }
        public Reaction Reaction { get; set; }
    }
}