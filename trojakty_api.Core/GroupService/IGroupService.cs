using System.Collections.Generic;
using System.Threading.Tasks;
using trojakty_api.Core.GroupService.DTOs;
using trojakty_api.Core.QuestionService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.GroupService
{
    public interface IGroupService
    {
        Task<Group> CreateGroupAsync(GroupDTO groupDto, User user);
        Task<List<Group>> GetGroupsAsync(User user);
        Task<Group> GetGroupAsync(int id, User user);
        Task<Group> InsertQuestionAsync(Group group, QuestionDTO questionDto);
        Task<Group> InsertUserAsync(Group group, User user);
        Task<Group> PublishAsync(Group group);
        Task<Group> RemoveUserAsync(Group group, User user);
        Task<Group> RemoveQuestionAsync(Group group, Question question);
    }
}