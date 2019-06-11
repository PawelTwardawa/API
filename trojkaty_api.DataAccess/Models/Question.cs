using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace trojkaty_api.DataAccess.Models
{
    public class Question
    {
        public Question()
        {
            Groups = new HashSet<GroupQuestion>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public bool Public { get; set; }
        [Required]
        public string QuestionText { get; set; }
        [Required]
        public string CorrectAnswer { get; set; }
        [Required]
        public string IncorrectAnswer1 { get; set; }
        [Required]
        public string IncorrectAnswer2 { get; set; }
        [Required]
        public string IncorrectAnswer3 { get; set; }
        public Category Category { get; set; }

        public ICollection<GroupQuestion> Groups { get; set; }
    }
}
