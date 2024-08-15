using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DTOs
{
    public class UserUpdateDTO   //kullanıcı güncelleme içerisinde bakiye güncellemesi yapıldı.
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; } 
    }
}