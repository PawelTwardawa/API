using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trojakty_api.Core.QuestionService;
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

        public async Task<List<Question>> GetAll()
        {
            return await Task.Run(() =>
            {
                return _validateQuestionRepository.FindBy(x => x.Validated == false).Include(x => x.Question)
                    .Select(s => s.Question).Include(x => x.Category).Where(x => x.Category != null).ToList();
            });

        }

        public async Task<ValidateQuestion> ConfirmValidation(Question question, bool publish)
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
