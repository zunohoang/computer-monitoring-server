namespace ComputerMonitoringServerAPI.Dtos
{
    public class UserDto
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
