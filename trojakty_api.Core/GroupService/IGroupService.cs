using System.Collections.Generic;
using System.Threading.Tasks;
using trojakty_api.Core.GroupService.DTOs;
using trojakty_api.Core.QuestionService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.GroupService
{
    public interface IGroupService
    {
        Task<Group> CreateGroup(GroupDTO groupDto, User user);
        Task<List<Group>> GetGroups(User user);
        Task<Group> GetGroup(int id, User user);
        Task<Group> InsertQuestion(Group group, QuestionDTO questionDto);
        Task<Group> InsertUser(Group group, User user);
        Task<Group> Publish(Group group);
        Task<Group> RemoveUser(Group group, User user);
        Task<Group> RemoveQuestion(Group group, Question question);
    }
}