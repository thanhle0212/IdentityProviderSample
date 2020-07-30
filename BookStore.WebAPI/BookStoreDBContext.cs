using BookStore.WebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebAPI
{
    public class BookStoreDBContext : DbContext
    {
        public BookStoreDBContext(DbContextOptions<BookStoreDBContext> context) : base(context)
        {

        }

        public DbSet<Book> Books { get; set; }
    }
}
