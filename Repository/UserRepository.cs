using Microsoft.EntityFrameworkCore;
using XTracker.Context;
using XTracker.DTOs.UserDTOs;
using XTracker.Repository.Interfaces;

namespace XTracker.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _uof;

        public UserRepository(AppDbContext context, IUnityOfWork uof)
        {
            _context = context;
            _uof = uof;
        }


        public async Task<UserDTO> UserDetails(string userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => new UserDTO
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    CreatedAt = u.CreatedAt,
                })
                .FirstOrDefaultAsync();
        }
    }
}
