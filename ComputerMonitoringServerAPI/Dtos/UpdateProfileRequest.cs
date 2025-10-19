using System.ComponentModel.DataAnnotations;

namespace ComputerMonitoringServerAPI.Dtos
{
    public class UpdateProfileRequest
    {
        [StringLength(255, MinimumLength = 2, ErrorMessage = "Tên đầy đủ phải từ 2-255 ký tự")]
        public string? FullName { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }
    }
}
