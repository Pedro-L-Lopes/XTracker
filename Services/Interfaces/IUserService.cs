using XTracker.DTOs.UserDTOs;

namespace XTracker.Services.Interfaces
{
    public interface IUserService
    {
        Task <UserDTO> UserDetails(string userId);
    }
}
