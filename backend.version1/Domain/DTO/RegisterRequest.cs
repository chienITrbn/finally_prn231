using System.ComponentModel.DataAnnotations;

namespace backend.version1.Domain.DTO
{
    public class RegisterRequest
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}