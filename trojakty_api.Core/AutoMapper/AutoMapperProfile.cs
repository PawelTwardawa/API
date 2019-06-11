using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using trojakty_api.Core.GroupService.DTOs;
using trojakty_api.Core.QuestionService.DTOs;
using trojakty_api.Core.UserService.DTOs;
using trojakty_api.Core.ValidateService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserDTO, User>();
            CreateMap<QuestionDTO, Question>();
            CreateMap<Question, QuestionDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.CategoryId, o => o.MapFrom(s => s.Category.Id))
                .ForMember(d => d.CorrectAnswer, o => o.MapFrom(s => s.CorrectAnswer))
                .ForMember(d => d.IncorrectAnswer1, o => o.MapFrom(s => s.IncorrectAnswer1))
                .ForMember(d => d.IncorrectAnswer2, o => o.MapFrom(s => s.IncorrectAnswer2))
                .ForMember(d => d.IncorrectAnswer3, o => o.MapFrom(s => s.IncorrectAnswer3))
                .ForMember(d => d.Public, o => o.MapFrom(s => s.Public))
                .ForMember(d => d.Question, o => o.MapFrom(s => s.QuestionText));
            CreateMap<Group, GroupResponseDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name));
            CreateMap<GroupDTO, Group>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Name))
                .ForMember(d => d.Questions, o => o.MapFrom(s => s.Questions));
            CreateMap<ValidateQuestion, ValidateResponseDTO>()
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.Published, o => o.MapFrom(s => s.Published))
                .ForMember(d => d.Question, o => o.MapFrom(s => s.Question))
                .ForMember(d => d.Validated, o => o.MapFrom(s => s.Validated));
        }
    }
}
