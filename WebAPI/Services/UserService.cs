using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity; //şifreyi hashleyebilmek için
using WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.DTOs;
using WebAPI.Data;
namespace WebAPI.Services
{

    public class UserService
    {
        private readonly ApplicationDbContext _context;
        private readonly PasswordHasher<User> _passwordHasher;
        public UserService(ApplicationDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<User>();
        }


        public async Task<User> RegisterUserAsync(UserDTO userDTO)
        {
            //kullanıcı kaydı için yazılmış fonksiyon
            var user = new User
            {
                Username = userDTO.Username,
                Name = userDTO.Name,
                Surname = userDTO.Surname,
                Email = userDTO.Email,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            user.Password = _passwordHasher.HashPassword(user, userDTO.Password);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            //Kullanıcı girişi yaparken kimliği doğrulamak için yazıldı
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return result == PasswordVerificationResult.Success;
        }

        public async Task<(bool IsValid, string ErrorMessage)> ValidateUserAsync(UserDTO userDTO)
        {
            // kullanıcı kayıt olurken username ve email değişkenlerinin eşsizliğini doğrulamak için yazıldı.
            if (await _context.Users.AnyAsync(u => u.Email == userDTO.Email))
            {
                return (false, "Bu e-posta ile kayıtlı bir kullanıcı zaten var.");
            }

            
            if (await _context.Users.AnyAsync(u => u.Username == userDTO.Username))
            {
                return (false, "Bu kullanıcı adı ile kayıtlı bir kullanıcı zaten var.");
            }

            return (true, null);
        }


        public async Task<(bool IsSuccess, string ErrorMessage, User User)> CreateUserAsync(UserDTO userDTO)
        {
            
            
            var validationResult = await ValidateUserAsync(userDTO); 
            // kullanıcı hesabı oluştururken username ve email değişkenlerindeki eşsizliğin kontrolu
            if (!validationResult.IsValid)
            {
                return (false, validationResult.ErrorMessage, null);
            }

            
            var user = await RegisterUserAsync(userDTO); 
            // kullanıcı username ve email değişkenlerindeki eşsizliği sağladıysa veri tabanına kaydedilir
            return (true, null, user);
        }

        public async Task<User> GetUserByIdAsync(int userId)
        {
            // veri tabanından UserID ye göre kullanıcınin bilgilerini getirir
            return await _context.Users.FindAsync(userId);
        }



        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            // bütün kullanıcıların bilgilerini getirir 
            return await _context.Users.ToListAsync();
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            // Belirtilen UserID'ye sahip kullanıcıyı siler
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }


        public async Task<bool> UpdateBalanceAsync(int userId, decimal newBalance)
        {
            // Bakiyenin negatif olmasını engellemek için kontrol
            if (newBalance < 0)
            {
                return false; // veya uygun bir hata mesajı dönebilirsiniz
            }

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return false; // Kullanıcı bulunamadı
            }

            user.Balance = newBalance;
            user.UpdatedAt = DateTime.Now;

            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }



    }
} 