using System.Threading.Tasks;

namespace trojakty_api.Core.StatisticService
{
    public interface IStatisticService
    {
        Task<int> QuestionsCountAsync();
        Task<int> CategoriesCountAsync();
        Task<int> UsersCountAsync();
        Task<int> GroupsCountAsync();
    }
}