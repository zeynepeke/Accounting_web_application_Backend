using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Models
{
    public class Revenue
    {
        public int RevenueId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }
        public string Source { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public User User { get; set; }
    }
}