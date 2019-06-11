using System;
using System.Collections.Generic;
using System.Text;
using trojakty_api.Core.QuestionService.DTOs;

namespace trojakty_api.Core.ValidateService.DTOs
{
    public class ValidateResponseDTO
    {
        public int Id { get; set; }
        public bool Validated { get; set; }
        public QuestionDTO Question { get; set; }
        public bool Published { get; set; }
    }
}
