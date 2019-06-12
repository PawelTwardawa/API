using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trojakty_api.Core.QuestionService;
using trojakty_api.Core.ValidateService.DTOs;
using trojkaty_api.DataAccess.Models;
using trojkaty_api.DataAccess.Repositories;

namespace trojakty_api.Core.ValidateService
{
    public class ValidateService : IValidateService
    {
        private IQuestionService _questionService;
        private IGenericRepository<ValidateQuestion> _validateQuestionRepository;
        private IGenericRepository<Question> _questionRepository;

        public ValidateService(IGenericRepository<ValidateQuestion> validateQuestionRepository,
            IGenericRepository<Question> questionRepository,
            IQuestionService questionService)
        {
            _validateQuestionRepository = validateQuestionRepository;
            _questionService = questionService;
            _questionRepository = questionRepository;
        }

        //public async Task<List<Question>> GetAllAsync()
        public async Task<List<ValidatedQuestionDTO>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                List<ValidatedQuestionDTO> result = new List<ValidatedQuestionDTO>();

                var list = _validateQuestionRepository.FindBy(x => x.Validated == false).Include(x => x.Question)
                    .Select(s => s.Question).Include(x => x.Category).Where(x => x.Category != null).Select(x =>
                        new
                        {
                            x.Id, x.QuestionText, x.CorrectAnswer, x.IncorrectAnswer1, x.IncorrectAnswer2, x
                                .IncorrectAnswer3,
                            x.Category.Name 
                        }).ToList();

                foreach (var a in list)
                {
                    result.Add(new ValidatedQuestionDTO()
                    {
                        Id = a.Id, Question = a.QuestionText, CorrectAnswer = a.CorrectAnswer,
                        IncorrectAnswer1 = a.IncorrectAnswer1, IncorrectAnswer2 = a.IncorrectAnswer2,
                        IncorrectAnswer3 = a.IncorrectAnswer3, Category = a.Name
                    });
                }

                return result;
            });

        }

        public async Task<ValidateQuestion> ConfirmValidationAsync(Question question, bool publish)
        {
            return await Task.Run(async () =>
            {
                var v = _validateQuestionRepository.FindBy(x => x.Question == question).SingleOrDefault();

                v.Published = publish;
                v.Validated = true;

                _validateQuestionRepository.Edit(v);
                await _validateQuestionRepository.SaveAsync();
                return v;
            });
        }
    }
}
