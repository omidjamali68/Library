using System.Threading.Tasks;
using Library.Entities;
using Library.InfraStructure;
using Library.Infrastructure.Application;
using Library.Services.BookCategories.Contracts;

namespace Library.Services.BookCategories
{
    public class BookCategoryAppService : BookCategoryService 
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly BookCategoryRepository _repository;

        public BookCategoryAppService(UnitOfWork unitOfWork, BookCategoryRepository repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public async Task<int> AddCategory(AddBookCategoryDto dto)
        {
            BookCategory bookCategory = new BookCategory
            {
                Title=dto.Title
            };
            _repository.Add(bookCategory);
            await _unitOfWork.Complete();
            return bookCategory.Id;
        }
    }
}
