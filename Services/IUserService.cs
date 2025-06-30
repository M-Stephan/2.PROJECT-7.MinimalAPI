using Solution.DTOs;
using Solution.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Solution.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsers();
        Task<UserDTO?> GetUser(int id);
        Task<UserDTO> CreateUser(User user);
        Task<UserDTO?> UpdateUser(int id, User updatedUser);
        Task<bool> DeleteUser(int id);
        Task<User?> Authenticate(string email, string password);
        string GenerateJwtToken(User user);
    }
}
