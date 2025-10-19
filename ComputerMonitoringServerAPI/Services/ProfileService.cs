using ComputerMonitoringServerAPI.Dtos;
using ComputerMonitoringServerAPI.Models;
using ComputerMonitoringServerAPI.Repositories;

namespace ComputerMonitoringServerAPI.Services
{
    public interface IProfileService
    {
        Task<UserDto?> GetProfileAsync(long userId);
        Task<UserDto?> UpdateProfileAsync(long userId, UpdateProfileRequest request);
        Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword);
    }

    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly IUserRepository _userRepository;

        public ProfileService(IProfileRepository profileRepository, IUserRepository userRepository)
        {
            _profileRepository = profileRepository;
            _userRepository = userRepository;
        }

        public async Task<UserDto?> GetProfileAsync(long userId)
        {
            var user = await _profileRepository.GetProfileAsync(userId);
            if (user == null)
                return null;

            return MapToUserDto(user);
        }

        public async Task<UserDto?> UpdateProfileAsync(long userId, UpdateProfileRequest request)
        {
            try
            {
                var user = await _profileRepository.GetProfileAsync(userId);
                if (user == null)
                    return null;

                // Kiểm tra email mới nếu thay đổi
                if (!string.IsNullOrEmpty(request.Email) && request.Email != user.Email)
                {
                    var existingEmail = await _userRepository.GetByEmailAsync(request.Email);
                    if (existingEmail != null)
                        throw new Exception("Email này đã được sử dụng");
                    user.Email = request.Email;
                }

                // Cập nhật full name
                if (!string.IsNullOrEmpty(request.FullName))
                {
                    user.FullName = request.FullName;
                }

                await _profileRepository.UpdateProfileAsync(user);
                await _profileRepository.SaveChangesAsync();

                return MapToUserDto(user);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật profile: {ex.Message}");
            }
        }

        public async Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _profileRepository.GetProfileAsync(userId);
                if (user == null)
                    return false;

                // Kiểm tra mật khẩu hiện tại
                if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                    throw new Exception("Mật khẩu hiện tại không chính xác");

                // Cập nhật mật khẩu mới
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await _profileRepository.UpdateProfileAsync(user);
                await _profileRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi đổi mật khẩu: {ex.Message}");
            }
        }

        private UserDto MapToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FullName = user.FullName,
                Role = user.Role,
                CreatedAt = user.CreatedAt
            };
        }
    }
}
