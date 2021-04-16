using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Library.Infrastructure.Application;

namespace Library.Services.Books.Contracts
{
    public interface BookService : Service
    {
        Task<int> AddBook(AddBookDto dto);
        Task<int> UpdateBook(int id, UpdateBookDto dto);
        Task<IEnumerable<Book>> GetBooksByCategoryId(int bookCategoryId);
        Task<Book> FindBookById(int id);
    }
}
