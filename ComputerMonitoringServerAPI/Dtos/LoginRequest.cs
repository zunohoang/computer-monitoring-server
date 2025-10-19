using System.ComponentModel.DataAnnotations;

namespace ComputerMonitoringServerAPI.Dtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username là bắt buộc")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string? Password { get; set; }
    }
}
