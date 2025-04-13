using Microsoft.AspNetCore.Mvc;
using SideBySideAPI.Interfaces;
using SideBySideAPI.Models.DTOs;
using SideBySideAPI.Models.Responses;

namespace SideBySideAPI.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUserService userService,
            IJwtService jwtService,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _jwtService = jwtService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerDto)
        {
            try
            {
                // Check if user with the same email already exists
                var existingUser = await _userService.GetUserByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Success = false,
                        Message = "User with this email already exists"
                    });
                }

                // Check if username is already taken
                var userWithSameUsername = await _userService.GetUserByUsernameAsync(registerDto.Username);
                if (userWithSameUsername != null)
                {
                    return BadRequest(new ErrorResponse
                    {
                        Success = false,
                        Message = "Username is already taken"
                    });
                }

                var createdUser = await _userService.CreateUserAsync(registerDto);
                var token = _jwtService.GenerateJwtToken(createdUser);

                return Ok(ApiResponse<TokenResponseDTO>.SuccessResponse(
                    new TokenResponseDTO { 
                        Token = token,
                        User = new UserDTO
                        {
                            Id = createdUser.Id,
                            Username = createdUser.Username,
                            Email = createdUser.Email,
                            FirstName = createdUser.FirstName,
                            LastName = createdUser.LastName,
                            Role = createdUser.Role.ToString()
                        }
                    }, 
                    "User registered successfully"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during user registration");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "An error occurred during registration"
                });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var user = await _userService.AuthenticateAsync(loginDto.Email, loginDto.Password);
                if (user == null)
                {
                    return Unauthorized(new ErrorResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    });
                }

                var token = _jwtService.GenerateJwtToken(user);

                return Ok(ApiResponse<TokenResponseDTO>.SuccessResponse(
                    new TokenResponseDTO
                    {
                        Token = token,
                        User = new UserDTO
                        {
                            Id = user.Id,
                            Username = user.Username,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Role = user.Role.ToString()
                        }
                    },
                    "Login successful"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, new ErrorResponse
                {
                    Success = false,
                    Message = "An error occurred during login"
                });
            }
        }
    }
} 