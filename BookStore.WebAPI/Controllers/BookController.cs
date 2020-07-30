using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.WebAPI.Entities;
using BookStore.WebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private IBookRepository _bookRepository;
        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }


        // GET: api/Book
        [HttpGet]
        [Authorize(Policy = "CanViewBook")]
        public IEnumerable<Book> Get()
        {
            var listBooks = _bookRepository.GetAllBooks();
            return listBooks;
        }

        // GET: api/Book/5
        [HttpGet("{id}", Name = "Get")]
        public Book Get(int id)
        {
            var book = _bookRepository.GetBookDetails(id);
            return book;
        }

        // POST: api/Book
        [HttpPost]
        public void Post([FromBody]Book model)
        {
            _bookRepository.AddBook(model);
        }
    }
}
