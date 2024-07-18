using XTracker.DTOs.UserDTOs;

namespace XTracker.Services.Interfaces
{
    public interface IUserService
    {
        Task <UserDTO> UserDetails(string userId);
        Task<bool> UpdateUser(UpdateUserDTO updateUserDTO);
    }
}
