using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace trojkaty_api.DataAccess.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public ICollection<GroupQuestion> Questions { get; set; }
        public ICollection<UserGroup> Users { get; set; }
    }
}
