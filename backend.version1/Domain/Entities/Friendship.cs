using Domain.Entities;

namespace backend.version1.Domain.Entities
{
    public class Friendship : BaseEntity
    {
        public string UserId { get; set; }
        public string FriendId { get; set; }
        public string Status { get; set; } = "Pending";
        public User User { get; set; }
        public User Friend { get; set; }
    }
}