using ComputerMonitoringServerAPI.Dtos;
using ComputerMonitoringServerAPI.Models;
using ComputerMonitoringServerAPI.Repositories;

namespace ComputerMonitoringServerAPI.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterRequest request);
        Task<AuthResponse> LoginAsync(LoginRequest request);
        Task<UserDto?> GetUserByIdAsync(long userId);
        Task<UserDto?> GetUserByUsernameAsync(string username);
        Task<AuthResponse> RefreshTokenAsync(string refreshToken);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserRepository userRepository, IJwtService jwtService, IConfiguration configuration, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                // Kiểm tra username đã tồn tại
                var existingUser = await _userRepository.GetByUsernameAsync(request.Username!);
                if (existingUser != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username đã tồn tại"
                    };
                }

                // Kiểm tra email đã tồn tại
                var existingEmail = await _userRepository.GetByEmailAsync(request.Email!);
                if (existingEmail != null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Email đã được đăng ký"
                    };
                }

                // Tạo user mới
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    FullName = request.FullName,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Role = "participant",
                    CreatedAt = DateTime.UtcNow,
                    NewColumn = 0
                };

                await _userRepository.AddAsync(user);
                await _userRepository.SaveChangesAsync();

                return new AuthResponse
                {
                    Success = true,
                    Message = "Đăng ký thành công",
                    User = MapToUserDto(user)
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Lỗi đăng ký: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username!);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username hoặc mật khẩu không chính xác"
                    };
                }

                // Kiểm tra mật khẩu
                if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Username hoặc mật khẩu không chính xác"
                    };
                }

                return new AuthResponse
                {
                    Success = true,
                    Message = "Đăng nhập thành công",
                    User = MapToUserDto(user),
                    Token = _jwtService.GenerateToken(user)
                };
            }
            catch (Exception ex)
            {
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Lỗi đăng nhập: {ex.Message}"
                };
            }
        }

        public async Task<AuthResponse> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                var principal = _jwtService.ValidateToken(refreshToken);
                if (principal == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Refresh token không hợp lệ"
                    };
                }

                var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !long.TryParse(userIdClaim.Value, out var userId))
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Token không có thông tin user"
                    };
                }

                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    return new AuthResponse
                    {
                        Success = false,
                        Message = "Người dùng không tồn tại"
                    };
                }

                return new AuthResponse
                {
                    Success = true,
                    Message = "Token đã được làm mới",
                    User = MapToUserDto(user),
                    Token = _jwtService.GenerateToken(user)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error refreshing token: {ex.Message}");
                return new AuthResponse
                {
                    Success = false,
                    Message = $"Lỗi làm mới token: {ex.Message}"
                };
            }
        }

        public async Task<UserDto?> GetUserByIdAsync(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user != null ? MapToUserDto(user) : null;
        }

        public async Task<UserDto?> GetUserByUsernameAsync(string username)
        {
            var user = await _userRepository.GetByUsernameAsync(username);
            return user != null ? MapToUserDto(user) : null;
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
