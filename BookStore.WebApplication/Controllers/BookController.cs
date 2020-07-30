using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using BookStore.WebApplication.Models;
using BookStore.WebApplication.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.WebApplication.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookStoreAPIService _bookAPIService;
        public BookController(IBookStoreAPIService bookAPIService)
        {
            _bookAPIService = bookAPIService;
        }

        [Authorize(Roles = "Viewer, Administrator")]
        public async Task<IActionResult> Index()
        {
            var listBooks = new List<BookModel>();
            var response = await _bookAPIService.GetBooks();

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                listBooks = JsonConvert.DeserializeObject<List<BookModel>>(json);
                return View(listBooks);
            }

            return Problem();
        }

        [Authorize(Policy = "CanAddBook")]
        [Authorize(Policy = "StartedYear")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Policy = "CanAddBook")]
        [Authorize(Policy = "StartedYear")]
        public async Task<IActionResult> Create(BookModel model)
        {
            var response = await _bookAPIService.AddBook(model);
            using (var client = new HttpClient())

                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return Unauthorized();
                }

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("Index");
            }

            return Problem();
        }

        [Route("book/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var book = new BookModel();
            var response = await _bookAPIService.BookDetails(id);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return Unauthorized();
            }
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var json = await response.Content.ReadAsStringAsync();
                book = JsonConvert.DeserializeObject<BookModel>(json);
                return View(book);
            }
            return Problem();
        }
    }
}