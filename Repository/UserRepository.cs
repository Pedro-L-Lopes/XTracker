using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using XTracker.Context;
using XTracker.DTOs.UserDTOs;
using XTracker.Repository.Interfaces;
using System.Threading.Tasks;
using XTracker.Models.Users;

namespace XTracker.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IUnityOfWork _uof;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(AppDbContext context, IUnityOfWork uof, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _uof = uof;
            _userManager = userManager;
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

        public async Task<bool> UpdateUser(UpdateUserDTO updateUserDTO)
        {
            var user = await _userManager.FindByIdAsync(updateUserDTO.UserId);

            if (user == null)
            {
                return false;
            }

            // Verificar a senha atual apenas se for fornecida uma nova senha
            if (!string.IsNullOrEmpty(updateUserDTO.NewPassword))
            {
                if (!await _userManager.CheckPasswordAsync(user, updateUserDTO.CurrentPassword!))
                {
                    return false;
                }

                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, updateUserDTO.CurrentPassword!, updateUserDTO.NewPassword);
                if (!passwordChangeResult.Succeeded)
                {
                    return false;
                }
            }

            // Atualizar o nome de usuário se fornecido
            if (!string.IsNullOrEmpty(updateUserDTO.UserName))
            {
                user.UserName = updateUserDTO.UserName;
            }

            // Atualizar o email se fornecido
            if (!string.IsNullOrEmpty(updateUserDTO.Email))
            {
                user.Email = updateUserDTO.Email;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            return updateResult.Succeeded;
        }

    }
}
