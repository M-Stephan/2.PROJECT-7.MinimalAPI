using Microsoft.EntityFrameworkCore;
using Solution.Data;
using Solution.Users;
using Solution.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Solution.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserDTO>> GetUsers()
        {
            var users = await _context.Users
                .Include(u => u.Tickets)
                .AsNoTracking()
                .ToListAsync();

            return users.Select(user => new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TicketTitles = user.Tickets != null
                    ? user.Tickets.Select(t => t.Title).ToList()
                    : new List<string>()
            });
        }

        public async Task<UserDTO?> GetUser(int id)
        {
            var user = await _context.Users
                .Include(u => u.Tickets)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null) return null;

            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TicketTitles = user.Tickets != null
                    ? user.Tickets.Select(t => t.Title).ToList()
                    : new List<string>()
            };
        }


        public async Task<UserDTO> CreateUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TicketTitles = new List<string>()
            };
        }

        public async Task<UserDTO?> UpdateUser(int id, User updatedUser)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            user.FirstName = updatedUser.FirstName;
            user.LastName = updatedUser.LastName;
            user.Email = updatedUser.Email;

            await _context.SaveChangesAsync();

            return new UserDTO
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                TicketTitles = new List<string>()
            };
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = new User { Id = id };
            _context.Entry(user).State = EntityState.Deleted;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
