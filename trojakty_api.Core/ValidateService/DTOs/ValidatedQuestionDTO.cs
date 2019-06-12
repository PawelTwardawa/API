using System;
using System.Collections.Generic;
using System.Text;

namespace trojakty_api.Core.ValidateService.DTOs
{
    public class ValidatedQuestionDTO
    {
        public int Id { get; set; }
            public string Question { get; set; }
            public string CorrectAnswer { get; set; }
            public string IncorrectAnswer1 { get; set; }
            public string IncorrectAnswer2 { get; set; }
            public string IncorrectAnswer3 { get; set; }
            public string Category { get; set; }
    }
}
