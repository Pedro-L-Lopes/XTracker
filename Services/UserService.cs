using AutoMapper;
using XTracker.DTOs.UserDTOs;
using XTracker.Repository.Interfaces;
using XTracker.Services.Interfaces;

namespace XTracker.Services
{
    public class UserService : IUserService
    {
        private readonly IUnityOfWork _uof;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IUnityOfWork uof, IMapper mapper, IUserRepository userRepository)
        {
            _uof = uof;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<UserDTO> UserDetails(string userId)
        {
            var user = await _userRepository.UserDetails(userId);
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<bool> UpdateUser(UpdateUserDTO updateUserDTO)
        {
            return await _userRepository.UpdateUser(updateUserDTO);
        }
    }
}
