using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Remotion.Linq.Parsing.Structure.IntermediateModel;
using trojkaty_api.DataAccess.Models;
using trojkaty_api.DataAccess.Repositories;

namespace trojakty_api.Core.StatisticService
{
    public class StatisticService : IStatisticService
    {
        private IGenericRepository<Question> _questionRepository;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<Category> _categoryRepository;
        private IGenericRepository<Group> _groupRepository;

        public StatisticService(
            IGenericRepository<User> userRepository,
            IGenericRepository<Question> questionRepository,
            IGenericRepository<Category> categoryRepository,
            IGenericRepository<Group> groupRepository)
        {
            _questionRepository = questionRepository;
            _categoryRepository = categoryRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
        }


        public async Task<int> QuestionsCountAsync()
        {
            return await Task.Run(() =>
            {
                return _questionRepository.GetAll().Count();
            });
        }

        public async Task<int> CategoriesCountAsync()
        {
            return await Task.Run(() =>
            {
                return _categoryRepository.GetAll().Count();
            });
        }

        public async Task<int> UsersCountAsync()
        {
            return await Task.Run(() =>
            {
                return _userRepository.GetAll().Count();
            });
        }

        public async Task<int> GroupsCountAsync()
        {
            return await Task.Run(() =>
            {
                return _groupRepository.GetAll().Count();
            });
        }


    }
}
