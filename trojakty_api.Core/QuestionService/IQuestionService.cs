using System.Collections.Generic;
using System.Threading.Tasks;
using trojakty_api.Core.QuestionService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.QuestionService
{
    public interface IQuestionService
    {
        Task<Category> CreateCategoryAsync(CategoryDTO categoryDTO);
        Task<Question> CreateQuestionAsync(QuestionDTO questionDTO);
        Task<Category> EditCategoryAsync(int id, CategoryDTO categoryDTO);
        Task<Question> EditQuestionAsync(int id, QuestionDTO questionDTO);
        Task<Category> GetCategoryAsync();
        Task<Category> GetCategoryAsync(int categoryId);
        Task<List<Category>> GetCategoriesAsync();
        Task<Question> GetQuestionAsync();
        Task<Question> GetQuestionAsync(Category category);
        Task<Question> GetQuestionAsync(int questionId);
        Task<List<Question>> GetQuestionsAsync(int count);
        Task<List<Question>> GetQuestionsAsync(int count, Category category);
        Task<List<Question>> GetQuestionsAsync(Group @group);
    }
}