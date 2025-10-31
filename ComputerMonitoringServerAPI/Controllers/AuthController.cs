using ComputerMonitoringServerAPI.Dtos;
using ComputerMonitoringServerAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ComputerMonitoringServerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Đăng ký tài khoản mới
        /// </summary>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation($"Đang xử lý đăng ký cho username: {request.Username}");

            var result = await _authService.RegisterAsync(request);

            if (!result.Success)
                return BadRequest(result);

            return CreatedAtAction(nameof(GetProfile), new { id = result.User?.Id }, result);
        }

        /// <summary>
        /// Đăng nhập
        /// </summary>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation($"Đang xử lý đăng nhập cho email: {request.Email}");

            var result = await _authService.LoginAsync(request);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }

        /// <summary>
        /// Lấy thông tin user theo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetProfile(long id)
        {
            var user = await _authService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = "Người dùng không tồn tại" });

            return Ok(user);
        }

        /// <summary>
        /// Lấy thông tin user theo username
        /// </summary>
        [HttpGet("username/{username}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UserDto>> GetByUsername(string username)
        {
            var user = await _authService.GetUserByUsernameAsync(username);
            if (user == null)
                return NotFound(new { message = "Người dùng không tồn tại" });

            return Ok(user);
        }

        /// <summary>
        /// Làm mới token (Refresh Token)
        /// </summary>
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AuthResponse>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _logger.LogInformation("Processing token refresh");

            var result = await _authService.RefreshTokenAsync(request.RefreshToken!);

            if (!result.Success)
                return Unauthorized(result);

            return Ok(result);
        }
    }
}
