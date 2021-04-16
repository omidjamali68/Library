using System.Collections.Generic;
using System.Threading.Tasks;
using Library.Entities;
using Library.Services.Books.Contracts;
using Library.Infrastructure.Application;
using Library.Services.Books.Exceptions;

namespace Library.Services.Books
{
    public class BookAppService : BookService
    {
        private readonly BookRepository _repository;
        private readonly UnitOfWork _unitOfWork;
        public BookAppService(BookRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddBook(AddBookDto dto)
        {
            Book book = new Book
            {
                Name = dto.Name,
                MinAgeNeed = dto.MinAgeNeed,
                Author = dto.Author,
                BookCategoryId = dto.CategoryId,
            };
            _repository.Add(book);
            await _unitOfWork.Complete();
            return book.Id;
        }

        public async Task<int> UpdateBook(int id, UpdateBookDto dto)
        {

            var existsBook = _repository.FindBookById(id);
            if (existsBook == null)
            {
                throw new FailedBookUpdateWhenBookNotExistsException();
            }
            existsBook.BookCategoryId = dto.CategoryId;
            existsBook.MinAgeNeed = dto.MinAgeNeed;
            existsBook.Author = dto.Author;
            existsBook.Name = dto.Name;
            await _unitOfWork.Complete();
            return existsBook.Id;
        }

        public async Task<IEnumerable<Book>> GetBooksByCategoryId(int bookCategoryId)
        {
            return await _repository.GetBooksByCategoryId(bookCategoryId);
        }

        public async Task<Book> FindBookById(int id)
        {
            return await Task.Run(() =>
                _repository.FindBookById(id)
            );
        }
    }
}
