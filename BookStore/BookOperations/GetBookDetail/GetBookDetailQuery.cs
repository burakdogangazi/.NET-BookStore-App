using System;
using System.Linq;
using AutoMapper;
using BookStore.Common;
using BookStore.DbOperations;

namespace BookStore.BookOperations.GetBookDetail
{
    public class GetBookDetailQuery
    {
        private readonly BookStoreDbContext _dbContext;

        public int BookId { get; set; }

        private readonly IMapper _mapper;

        public GetBookDetailQuery(BookStoreDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public BookDetailViewModel Handle()
        {
            var book = _dbContext.Books.SingleOrDefault(book => book.Id == BookId);

            if (book is null)
            {
                throw new InvalidOperationException("The book is not exist");
            }

            BookDetailViewModel vm = _mapper.Map<BookDetailViewModel>(book); //new BookDetailViewModel();
            /*vm.Title = book.Title;
            vm.PageCount = book.PageCount;
            vm.PublishDate = book.PublishDate.Date.ToString("dd/MM/yyyy");
            vm.Genre = ((GenreEnum)book.GenreId).ToString();*/

            return vm;
        }
    }

    public class BookDetailViewModel
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int PageCount { get; set; }
        public string PublishDate { get; set; }
    }
}