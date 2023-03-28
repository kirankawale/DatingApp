using System.ComponentModel.DataAnnotations;

namespace API.Entity
{
    public class AppUser
    {
        [Key]
        public int Id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }
        public byte[] SaltPassword { get; set; }
    }
}