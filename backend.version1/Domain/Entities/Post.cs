using Domain.Entities;

namespace backend.version1.Domain.Entities
{
    public class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Tags { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
        public ICollection<PostReaction> PostReactions { get; set; } = new List<PostReaction>();
        public ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();
    }
}