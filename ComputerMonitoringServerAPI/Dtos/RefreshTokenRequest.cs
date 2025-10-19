using System.ComponentModel.DataAnnotations;

namespace ComputerMonitoringServerAPI.Dtos
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "Refresh token là bắt buộc")]
        public string? RefreshToken { get; set; }
    }
}
