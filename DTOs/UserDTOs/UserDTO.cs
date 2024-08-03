namespace XTracker.DTOs.UserDTOs
{
    public class UserDTO
    {
        public string Id { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
