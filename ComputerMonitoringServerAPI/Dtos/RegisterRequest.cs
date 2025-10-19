using System.ComponentModel.DataAnnotations;

namespace ComputerMonitoringServerAPI.Dtos
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Username là bắt buộc")]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Username phải từ 3-255 ký tự")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(255, MinimumLength = 6, ErrorMessage = "Mật khẩu phải ít nhất 6 ký tự")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Tên đầy đủ là bắt buộc")]
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Tên đầy đủ phải từ 2-255 ký tự")]
        public string? FullName { get; set; }
    }
}
