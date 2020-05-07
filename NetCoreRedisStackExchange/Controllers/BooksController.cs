using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreRedisStackExchange.CacheService;
using NetCoreRedisStackExchange.Models;

namespace NetCoreRedisStackExchange.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private static List<Book> _books;
        private ICacheService _cacheService;

        public BooksController(ICacheService cacheService)
        {
            _cacheService = cacheService;

            InitBookList();
        }

        public IActionResult Index()
        {
            if (_cacheService.Any("books"))
            {
                var books = _cacheService.Get<List<Book>>("books");
                return Ok(books);
            }

            _cacheService.Add("books", _books);

            return Ok(_books);
        }

        [HttpPost]
        public IActionResult Add(Book book)
        {
            _books.Add(book);
            _cacheService.Remove("books");

            return Ok("Book successfully added.");
        }

        private void InitBookList()
        {
            if (_books == null)
            {
                _books = new List<Book>();
                _books.Add(new Book(1, "Suç ve Ceza", "Fyodor Dostoyevski"));
                _books.Add(new Book(2, "Satranç", "Stefan Zweig"));
                _books.Add(new Book(3, "Fahrenheit 451", "Ray Bradbury"));
                _books.Add(new Book(4, "Yaban", "Yakup Kadri Karaosmanoğlu"));
            }
        }
    }
}