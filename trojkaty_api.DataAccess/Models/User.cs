using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace trojkaty_api.DataAccess.Models
{
    public enum Role
    {
        User,
        Admin
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Role Role { get; set; }

        public ICollection<UserGroup> Groups { get; set; }
    }
}
