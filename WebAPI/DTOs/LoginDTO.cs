using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.DTOs
{
    public class LoginDTO
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
         public int? UserId { get; set; }
        public string? Name { get; private set; }
        public string? Surname { get; private set; }
    }
}