using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using BookStore.BookOperations.CreateBook;
using BookStore.BookOperations.DeleteBook;
using BookStore.BookOperations.GetBookDetail;
using BookStore.BookOperations.GetBooks;
using BookStore.DbOperations;
using Microsoft.AspNetCore.Mvc;
using BookStore.BookOperations.UpdateBook;
using FluentValidation;
using FluentValidation.Results;

namespace BookStore.Controllers
{
    [ApiController]
    [Route("[controller]s")]
    public class BookController : ControllerBase
    {
        private readonly BookStoreDbContext _context;

        //just can be setted on ctor with readonly, cannot set at another place
        private readonly IMapper _mapper;

        public BookController(BookStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetBooks()
        {
            GetBooksQuery query = new GetBooksQuery(_context, _mapper);
            var result = query.Handle();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            BookDetailViewModel result;
            GetBookDetailQuery query = new GetBookDetailQuery(_context, _mapper);

            try
            {
                query.BookId = id;
                result = query.Handle();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok(result);
        }

        /* [HttpGet]
         public Book Get([FromQuery]int id)
         {
             var book = BookList.Where(x => x.Id == Convert.ToInt32(id)).SingleOrDefault();
             return book;
         }*/

        [HttpPost]
        public IActionResult AddBook([FromBody] CreateBookCommand.CreateBookModel newBook)
        {
            CreateBookCommand command = new CreateBookCommand(_context, _mapper);
            try
            {
                command.Model = newBook;
                CreateBookCommandValidator createBookCommandValidator = new CreateBookCommandValidator();
                /*ValidationResult result = createBookCommandValidator.Validate(command);
                if (!result.IsValid)
                {
                    foreach (var item in result.Errors)
                    {
                        Console.WriteLine("Props: " + item.PropertyName + "- Error Message: " + item.ErrorMessage);
                    }
                }
                else
                {
                    command.Handle();
                }*/
                
                createBookCommandValidator.ValidateAndThrow(command);
                command.Handle();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(int id, [FromBody] UpdateBookCommand.UpdateBookModel updatedBook)
        {
            UpdateBookCommand command = new UpdateBookCommand(_context);

            try
            {
                command.BookId = id;
                command.Model = updatedBook;
                command.Handle();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(int id)
        {
            DeleteBookCommand command = new DeleteBookCommand(_context);
            try
            {
                command.BookId = id;
                DeleteBookCommandValidator deleteBookCommandValidator = new DeleteBookCommandValidator();
                deleteBookCommandValidator.ValidateAndThrow(command);
                command.Handle();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }
    }
}