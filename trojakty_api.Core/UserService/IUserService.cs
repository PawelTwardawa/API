using System.Collections.Generic;
using System.Threading.Tasks;
using trojakty_api.Core.UserService.DTOs;
using trojkaty_api.DataAccess.Models;

namespace trojakty_api.Core.UserService
{
    public interface IUserService
    {
        User Authenticate(UserDTO userDTO);
        Task<User> Create(UserDTO userDTO);
        void Delete(UserDTO userDTO);
        IEnumerable<User> GetAll();
        User GetByEmail(string email);
    }
}