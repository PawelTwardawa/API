using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using trojakty_api.Core.Exceptions;
using trojakty_api.Core.QuestionService.DTOs;
using trojkaty_api.DataAccess.Models;
using trojkaty_api.DataAccess.Repositories;

namespace trojakty_api.Core.QuestionService
{
    public class QuestionService : IQuestionService
    {
        private IMapper _mapper;
        private IGenericRepository<Question> _questionRepository;
        private IGenericRepository<Category> _categoryRepository;
        private IGenericRepository<GroupQuestion> _groupQuestionyRepository;

        public QuestionService(IMapper mapper, 
            IGenericRepository<Question> questionRepository,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<GroupQuestion> groupQuestionyRepository
            )
        {
            _mapper = mapper;
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _groupQuestionyRepository = groupQuestionyRepository;
        }

        private void ValidateAnswers(QuestionDTO questionDto)
        {
            if (questionDto == null) throw new TrojkatyCoreException("Question cannot be null");
            if (string.Equals(questionDto.CorrectAnswer, questionDto.IncorrectAnswer1, StringComparison.CurrentCultureIgnoreCase) ||
                string.Equals(questionDto.CorrectAnswer, questionDto.IncorrectAnswer2, StringComparison.CurrentCultureIgnoreCase) ||
                string.Equals(questionDto.CorrectAnswer, questionDto.IncorrectAnswer3, StringComparison.CurrentCultureIgnoreCase) ||
                string.Equals(questionDto.IncorrectAnswer1, questionDto.IncorrectAnswer2, StringComparison.CurrentCultureIgnoreCase) ||
                string.Equals(questionDto.IncorrectAnswer1, questionDto.IncorrectAnswer3, StringComparison.CurrentCultureIgnoreCase) ||
                string.Equals(questionDto.IncorrectAnswer2, questionDto.IncorrectAnswer3, StringComparison.CurrentCultureIgnoreCase))
                throw new TrojkatyCoreException("Answers couldn't be the same");
        }

        public async Task<Question> CreateQuestion(QuestionDTO questionDto)
        {
            Category category = null;
            if (questionDto.CategoryId != null)
                category = _categoryRepository.FindBy(x => x.Id == questionDto.CategoryId).SingleOrDefault();

            ValidateAnswers(questionDto);

            Question question = new Question();
            question.QuestionText = questionDto.Question;
            question.IncorrectAnswer1 =  questionDto.IncorrectAnswer1;
            question.IncorrectAnswer2 =  questionDto.IncorrectAnswer2;
            question.IncorrectAnswer3 =  questionDto.IncorrectAnswer3;
            question.CorrectAnswer = questionDto.CorrectAnswer;
            question.Category = category;
            question.Public = questionDto.Public;


            await _questionRepository.Add(question);
            await _questionRepository.SaveAsync();

            return question;
        }

        public async Task<Question> EditQuestion(int id, QuestionDTO questionDto)
        {
            var question = await GetQuestion(id);

            if (question == null)
                throw new TrojkatyCoreException($"Cannot find question on id = {id}");

            if (questionDto == null)
                throw new TrojkatyCoreException("Question DTOs is null");

            ValidateAnswers(questionDto);

            Category category = null;
            if (questionDto.CategoryId != null)
                category = _categoryRepository.FindBy(x => x.Id == questionDto.CategoryId).SingleOrDefault();

            question.QuestionText = questionDto.Question;
            question.IncorrectAnswer1 = questionDto.IncorrectAnswer1;
            question.IncorrectAnswer2 = questionDto.IncorrectAnswer2;
            question.IncorrectAnswer3 = questionDto.IncorrectAnswer3;
            question.CorrectAnswer = questionDto.CorrectAnswer;
            question.Category = category;
            question.Public = questionDto.Public;
            question.Id = id;

            _questionRepository.Edit(question);
            await _questionRepository.SaveAsync();

            return question;
        }

        public async Task<Question> GetQuestion()
        {
            Random random = new Random();

            return await Task.FromResult(

                _questionRepository.FindBy(x => x.Public == true).OrderBy(x => x.Id).Skip(random.Next(0, _questionRepository.GetAll().Count())).Include(x => x.Category).First()
            );
        }

        public async Task<List<Question>> GetQuestions(Group group)
        {
            return await Task.Run(() =>
            {
                return _groupQuestionyRepository.FindBy(x => x.Group == group).Include(x => x.Question)
                    .Select(x => x.Question).Include(x => x.Category).ToList();

                
            });
        }

        public async Task<List<Question>> GetQuestions(int count)
        {
            return await Task.Run(() =>
            {
                //return _questionRepository.GetAll().OrderBy(r => Guid.NewGuid()).Take(count).AsEnumerable().ToList();
                return _questionRepository.FindBy(x => x.Public == true).OrderBy(r => Guid.NewGuid()).Take(count).Include(x => x.Category).AsEnumerable().ToList();
            });
        }

        public async Task<List<Question>> GetQuestions(int count, Category category)
        {
            return await Task.Run(() =>
            {
                return _questionRepository.FindBy(x => x.Category == category).OrderBy(r => Guid.NewGuid()).Take(count).AsEnumerable().ToList();
            });
        }

        public async Task<Question> GetQuestion(Category category)
        {
            return await Task.Run(() =>
            {
                return _questionRepository.FindBy(x => x.Category == category).OrderBy(r => Guid.NewGuid()).Take(1).SingleOrDefault();
            });
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            return await Task.FromResult(

                _questionRepository.FindBy(x => x.Id == questionId).Include(x => x.Category).SingleOrDefault()
            );
        }

        public async Task<Category> CreateCategory(CategoryDTO categoryDto)
        {
            var cat = _categoryRepository.FindBy(x => x.Name == categoryDto.Category).SingleOrDefault();

            if (cat != null)
                throw new TrojkatyCoreException($"Category {categoryDto.Category} exist");

            Category category = new Category() { Name = categoryDto.Category };

            await _categoryRepository.Add(category);
            await _categoryRepository.SaveAsync();

            return category;
        }

        public async Task<Category> EditCategory(int id, CategoryDTO categoryDto)
        {
            var category = await GetCategory(id);
            if (category == null)
                throw  new TrojkatyCoreException($"Cannot find category on id = {id}");


            if(string.IsNullOrWhiteSpace(categoryDto.Category))
                throw  new TrojkatyCoreException("New category name cannot be empty or only white space");
            //Category category = new Category() {Id = id, Name = categoryDto.Category};
            //await Task.Run(() => _categoryRepository.Edit(category));

            category.Name = categoryDto.Category;

            _categoryRepository.Edit(category);
            await _categoryRepository.SaveAsync();

            return category;
        }

        public async Task<Category> GetCategory()
        {
            return await Task.FromResult(

                _categoryRepository.GetAll().OrderBy(r => Guid.NewGuid()).Take(1).SingleOrDefault()
            );
        }

        public async Task<Category> GetCategory(int categoryId)
        {
            return await Task.FromResult(

                _categoryRepository.FindBy(x => x.Id == categoryId).SingleOrDefault()
            );
        }

        public async Task<List<Category>> GetCategories()
        {
            return await Task.Run(() => _categoryRepository.GetAll().ToList());
        }
    }
}
