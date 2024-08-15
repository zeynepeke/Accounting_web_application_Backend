using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Services;
using WebAPI.DTOs;
using WebAPI.Models;

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
            // kullancı kayıdı için yazıldı
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
            // kayıtlı olan kullanıcı girişi için yazıldı
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Email) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return BadRequest("Geçersiz Email ya da şifre.");
            }

            var isValidUser = await _userService.ValidateUserAsync(loginDto.Email, loginDto.Password);

            if (isValidUser)
            {
                
                return Ok("Giriş Başarılı");
            }
            else
            {
                
                return Unauthorized("Geçersiz Email ya da şifre");
            }
        }




        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            // bütün kullanıcıların bilgilerini getirir 
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }




        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            // veri tabanında kayıtlı Id bilgisine göre ilgili kullanıcıyı getirir 
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

        //id ye bağlı bakiyeyi güncelliyor.

        [HttpPut("update-balance/{id}")]
        public async Task<IActionResult> UpdateBalance(int userId, [FromBody] decimal newBalance)
        {
            var result = await _userService.UpdateBalanceAsync(userId, newBalance);
            if (result)
            {
                return Ok("Balance updated successfully.");
            }
            else
            {
                return BadRequest("Failed to update balance. Ensure the balance is not negative and user exists.");
            }
        }



    }
}