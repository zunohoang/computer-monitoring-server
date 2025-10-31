using System.ComponentModel.DataAnnotations;

namespace ComputerMonitoringServerAPI.Dtos
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        //[Required(ErrorMessage = "Username là bắt buộc")]
        //public string? Username { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        public string? Password { get; set; }
    }
}
