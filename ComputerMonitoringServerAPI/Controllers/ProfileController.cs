using ComputerMonitoringServerAPI.Dtos;
using ComputerMonitoringServerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ComputerMonitoringServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileService _profileService;
        private readonly ILogger<ProfileController> _logger;

        public ProfileController(IProfileService profileService, ILogger<ProfileController> logger)
        {
            _profileService = profileService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy thông tin profile của user
        /// </summary>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetProfile(long userId)
        {
            _logger.LogInformation($"Lấy profile cho userId: {userId}");

            var profile = await _profileService.GetProfileAsync(userId);
            if (profile == null)
                return NotFound(new { message = "Profile không tồn tại" });

            return Ok(profile);
        }

        /// <summary>
        /// Cập nhật thông tin profile
        /// </summary>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> UpdateProfile(long userId, [FromBody] UpdateProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation($"Cập nhật profile cho userId: {userId}");

            try
            {
                var updatedProfile = await _profileService.UpdateProfileAsync(userId, request);
                if (updatedProfile == null)
                    return NotFound(new { message = "Profile không tồn tại" });

                return Ok(new { message = "Cập nhật profile thành công", user = updatedProfile });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi cập nhật profile: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        [HttpPost("{userId}/change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ChangePassword(long userId, [FromBody] ChangePasswordRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation($"Đổi mật khẩu cho userId: {userId}");

            try
            {
                var result = await _profileService.ChangePasswordAsync(userId, request.CurrentPassword!, request.NewPassword!);
                if (!result)
                    return BadRequest(new { message = "Không thể đổi mật khẩu" });

                return Ok(new { message = "Đổi mật khẩu thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Lỗi đổi mật khẩu: {ex.Message}");
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
