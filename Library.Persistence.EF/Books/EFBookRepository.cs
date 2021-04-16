using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Library.Services.Books.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistence.EF.Books
{
    public class EFBookRepository : BookRepository
    {
        private readonly EFDataContext _context;

        public EFBookRepository(EFDataContext context)
        {
            _context = context;
        }
        public void Add(Book book)
        {
            _context.Books.Add(book);
        }

        public Book FindBookById(int id)
        {
            var book = _context.Books.SingleOrDefault(_=>_.Id==id);
            return book;
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryId(int categoryId)
        {
            return await _context.Books.Where(_ => _.BookCategoryId == categoryId).ToListAsync();
        }
    }
}
