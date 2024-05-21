using System.ComponentModel.DataAnnotations;

namespace backend.version1.Domain.DTO
{
    public class LoginRequest
    {
        [Required]
        public string? Username { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}