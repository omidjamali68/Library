using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Library.Infrastructure.Application;

namespace Library.Services.Books.Contracts
{
    public interface BookRepository : Repository
    {
        void Add(Book book);
        Book FindBookById(int id);

        Task<IEnumerable<Book>> GetBooksByCategoryId(int categoryId);
    }
}
