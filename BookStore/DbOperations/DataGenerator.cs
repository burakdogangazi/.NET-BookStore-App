using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace BookStore.DbOperations
{
    public class DataGenerator
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context =
                   new BookStoreDbContext(serviceProvider.GetRequiredService<DbContextOptions<BookStoreDbContext>>()))
            {
                if (context.Books.Any())
                {
                    return;
                }

                context.Books.AddRange(
                    new Book
                    {
                       // Id = 1,
                        Title = "Lean Startup",
                        GenreId = 1,
                        PageCount = 300,
                        PublishDate = new DateTime(2002, 06, 12)
                    },
                    new Book
                    {
                      //  Id = 2,
                        Title = "Herland",
                        GenreId = 2,
                        PageCount = 400,
                        PublishDate = new DateTime(2004, 06, 12)
                    },
                    new Book
                    {
                      //  Id = 3,
                        Title = "Dune",
                        GenreId = 3,
                        PageCount = 600,
                        PublishDate = new DateTime(2009, 06, 12)
                    });

                context.SaveChanges();
            }
        }
    }
}