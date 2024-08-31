using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using WebAPI.DTOs;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.CreateUserAsync(userDTO);
            if (!result.IsSuccess)
            {
                return Conflict(result.ErrorMessage);
            }

            return CreatedAtAction(nameof(GetUserById), new { id = result.User.UserId }, result.User);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Geçersiz Email ya da şifre.");
            }

            var user = await _userService.ValidateUserAsync(loginDto.Email, loginDto.Password);
            if (user != null)
            {
                return Ok(user);
            }

            return Unauthorized("Geçersiz Email ya da şifre");
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return NoContent();
        }

        [HttpPut("update-balance/{id}")]
        public async Task<IActionResult> UpdateBalance(int id, [FromBody] decimal newBalance)
        {
            var result = await _userService.UpdateBalanceAsync(id, newBalance);
            if (result)
            {
                return Ok("Balance updated successfully.");
            }

            return BadRequest("Failed to update balance. Ensure the balance is not negative and user exists.");
        }

        [HttpGet("profile/{id}")]
        public async Task<IActionResult> GetUserProfile(int id)
        {
            var userProfile = await _userService.GetUserByIdAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }

            return Ok(userProfile);
        }

[HttpPut("update-profile/{id}")]
public async Task<IActionResult> UpdateProfile(int id, [FromBody] UserDTO userDTO)
{
    var updatedUser = await _userService.UpdateUserProfileAsync(id, userDTO);
    if (updatedUser != null)
    {
        return Ok(updatedUser); // Güncellenen kullanıcı profilini döndür
    }

    return BadRequest("Failed to update profile.");
}


[HttpPost("delete-profile/{id}")]
public async Task<IActionResult> DeleteProfile(int id, [FromBody] DeleteProfileDTO deleteProfileDTO)
{
    var result = await _userService.DeleteUserAsync(id, deleteProfileDTO.Password);
    if (result)
    {
        return NoContent();
    }
    return BadRequest("Failed to delete profile.");
}
       
        
        
        
        
        
        
        
        
        [HttpPut("profile/password/{id}")]
        public async Task<IActionResult> UpdatePassword(int id, [FromBody] UpdatePasswordDTO updatePasswordDTO)
        {
            var result = await _userService.UpdatePasswordAsync(id, updatePasswordDTO.OldPassword, updatePasswordDTO.NewPassword);
            if (result)
            {
                return Ok("Password updated successfully.");
            }

            return BadRequest("Failed to update password.");
        }

     
    }
}
