using BookStore.WebApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.WebApplication.Services
{
    public class BookStoreAPIService : IBookStoreAPIService
    {
        private readonly HttpClient _client;

        public BookStoreAPIService(HttpClient client)
        {
            _client = client;
        }
        public async Task<HttpResponseMessage> AddBook(BookModel model)
        {
            var request = JsonConvert.SerializeObject(model);
            HttpContent requestBody = new StringContent(request, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("book", requestBody);
            return response;
        }

        public async Task<HttpResponseMessage> BookDetails(int id)
        {
            var response = await _client.GetAsync($"book/{id}");
            return response;
        }

        public async Task<HttpResponseMessage> GetBooks()
        {
            var response = await _client.GetAsync("book");
            return response;
        }
    }

    public interface IBookStoreAPIService
    {
         Task<HttpResponseMessage> GetBooks();
         Task<HttpResponseMessage> BookDetails(int id);
         Task<HttpResponseMessage> AddBook(BookModel model);
    }
}
