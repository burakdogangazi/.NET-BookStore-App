using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {

        private static List<Book> BookList = new List<Book>()
        {
            new Book
            {
                Id = 1,
                Title = "Lean Startup",
                GenreId = 1,
                PageCount = 300,
                PublishDate = new DateTime(2002, 06, 12)
            },
            new Book
            {
                Id = 2,
                Title = "Herland",
                GenreId = 2,
                PageCount = 400,
                PublishDate = new DateTime(2004, 06, 12)
            },
            new Book
            {
                Id = 3,
                Title = "Dune",
                GenreId = 3,
                PageCount = 600,
                PublishDate = new DateTime(2009, 06, 12)
            }
        };
        
        [HttpGet]
        public List<Book> GetBooks()
        {
            var bookList = BookList.OrderBy(x => x.Id).ToList();
            return bookList;
        }
        
        [HttpGet("{id}")]
        public Book GetById(int id)
        {
            var book = BookList.Where(x => x.Id == id).SingleOrDefault();
            return book;
        }
        
        
        [HttpGet]
        public Book Get([FromQuery]int id)
        {
            var book = BookList.Where(x => x.Id == Convert.ToInt32(id)).SingleOrDefault();
            return book;
        }
        
        
    }
}