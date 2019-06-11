using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace trojkaty_api.DataAccess.Models
{
    public class ValidateQuestion
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public bool Validated { get; set; }
        //[Required]
        public Question Question { get; set; }
        [Required]
        public bool Published { get; set; }
    }
}
