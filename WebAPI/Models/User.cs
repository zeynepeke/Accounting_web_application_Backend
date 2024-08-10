using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }  
        public string Surname { get; set; }  
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Invoice> Invoices { get; set; }
        public ICollection<Expense> Expenses { get; set; }
        public ICollection<Revenue> Revenues { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}