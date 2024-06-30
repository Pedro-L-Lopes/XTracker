using XTracker.DTOs.UserDTOs;

namespace XTracker.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<UserDTO> UserDetails(string userId);
    }
}
