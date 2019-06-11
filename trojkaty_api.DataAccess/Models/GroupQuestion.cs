using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace trojkaty_api.DataAccess.Models
{
    public class GroupQuestion
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
