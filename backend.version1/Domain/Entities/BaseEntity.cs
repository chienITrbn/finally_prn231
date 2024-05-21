using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; protected set; }

        public DateTime Created { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime? LastModified { get; set; }

        public string? LastModifiedBy { get; set; }

        protected BaseEntity()
        {
            Created = DateTime.Now;
        }
    }
}