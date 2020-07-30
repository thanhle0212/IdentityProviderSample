using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebApplication.Models
{
    public class BookModel
    {
        public int Id { get; set; }
        public string BookName { get; set; }
        public double Price { get; set; }
        public string Publisher { get; set; }
        public string Author { get; set; }
    }
}
