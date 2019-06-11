using System.Collections.Generic;
using System.Threading.Tasks;
using trojakty_api.Core.QuestionService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.QuestionService
{
    public interface IQuestionService
    {
        Task<Category> CreateCategory(CategoryDTO categoryDTO);
        Task<Question> CreateQuestion(QuestionDTO questionDTO);
        Task<Category> EditCategory(int id, CategoryDTO categoryDTO);
        Task<Question> EditQuestion(int id, QuestionDTO questionDTO);
        Task<Category> GetCategory();
        Task<Category> GetCategory(int categoryId);
        Task<List<Category>> GetCategories();
        Task<Question> GetQuestion();
        Task<Question> GetQuestion(Category category);
        Task<Question> GetQuestion(int questionId);
        Task<List<Question>> GetQuestions(int count);
        Task<List<Question>> GetQuestions(int count, Category category);
        Task<List<Question>> GetQuestions(Group @group);
    }
}