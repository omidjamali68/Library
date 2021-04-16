using Library.Entities;
using Library.InfraStructure;
using Library.Services.BookCategories.Contracts;

namespace Library.Persistence.EF.BookCategories
{
    public class EFBookCategoryRepository : BookCategoryRepository
    {
        private readonly EFDataContext _context;

        public EFBookCategoryRepository(EFDataContext context)
        {
            _context = context;
        }
        public void Add(BookCategory bookCategory)
        {
            _context.Add(bookCategory);
        }
    }
}
