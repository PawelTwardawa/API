using System;
using System.Collections.Generic;
using System.Text;
using trojakty_api.Core.QuestionService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.GroupService.DTOs
{
    public class GroupDTO
    {
        public string Name { get; set; }
        public List<QuestionDTO> Questions { get; set; }
    }
}
