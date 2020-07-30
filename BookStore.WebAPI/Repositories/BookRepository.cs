using BookStore.WebAPI.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.WebAPI.Repositories
{
    public class BookRepository : IBookRepository
    {
        private BookStoreDBContext _context;
        public BookRepository(BookStoreDBContext context)
        {
            _context = context;

            if (!_context.Books.Any())
            {

                _context.Books.Add(new Book
                {
                    BookName = "Tom and Jerry",
                    Price = 123.5,
                    Author = "Unknow",
                    Publisher = "Kim Dong"
                });

                _context.Books.Add(new Book
                {
                    BookName = "Steve Job",
                    Price = 200,
                    Author = "Unknow",
                    Publisher = "BBC"
                });

                _context.Books.Add(new Book
                {
                    BookName = "Golden Axe",
                    Price = 199,
                    Author = "David Copy and Paste",
                    Publisher = "SEGA"
                });
                _context.SaveChanges();
            }
        }
        public void AddBook(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public Book GetBookDetails(int bookId)
        {
            return _context.Books.FirstOrDefault(x => x.Id == bookId);
        }
    }

    public interface IBookRepository
    {
        void AddBook(Book book);
        Book GetBookDetails(int bookId);
        IEnumerable<Book> GetAllBooks();
    }
}
