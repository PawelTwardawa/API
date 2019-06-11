using System;
using System.Collections.Generic;
using System.Text;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.QuestionService.DTOs
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public string IncorrectAnswer1 { get; set; }
        public string IncorrectAnswer2 { get; set; }
        public string IncorrectAnswer3 { get; set; }
        public bool Public { get; set; }
        public int? CategoryId { get; set; }
    }
}
