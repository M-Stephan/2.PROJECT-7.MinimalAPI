using Solution.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solution.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User?> GetUser(int id);
        Task<User> CreateUser(User user);
        Task<User?> UpdateUser(int id, User updatedUser);
        Task<bool> DeleteUser(int id);
    }
}