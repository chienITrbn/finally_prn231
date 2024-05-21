using Microsoft.AspNetCore.Identity;

namespace backend.version1.Domain.Entities
{
    public class User : IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        public ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public ICollection<Comment> Comments { get; set; } = new HashSet<Comment>();
        public ICollection<Reaction> Reactions { get; set; } = new HashSet<Reaction>();

        //public ICollection<Friendship> Friendships { get; set; } = new HashSet<Friendship>();
    }
}