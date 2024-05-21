using Domain.Entities;

namespace backend.version1.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }

        public ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();
    }
}