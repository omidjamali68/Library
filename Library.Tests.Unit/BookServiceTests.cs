using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Entities;
using Library.Persistence.EF;
using Library.Persistence.EF.Books;
using Library.Services.Books;
using Library.Services.Books.Contracts;
using Library.Services.Books.Exceptions;
using Library.TestTools.BookCategories;
using Library.TestTools.Books;
using Work.Infrastructure.Test;
using Xunit;

namespace Library.Tests.Unit
{
    public class BookServiceTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly BookService _sut;

        public BookServiceTests()
        {
            var db=new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository=new EFBookRepository(_context);
            var unitOfWork=new EFUnitOfWork(_context);
            _sut=new BookAppService(repository,unitOfWork);
        }

        [Fact]
        public async void Add_add_book_properly()
        {
            BookCategory bookCategory=BookCategoryFactory.GenerateBookCategory();
            _context.Manipulate(_=>_.BookCategories.Add(bookCategory));
            Book book = new BookBuilder()
                .BuildBookWithCategoryId(bookCategory.Id)
                .Build();
            var dto = BookDtoFactory.GenerateAddBookDto(book.BookCategoryId, book.MinAgeNeed
                , book.Author, book.Name);

            var actual =await _sut.AddBook(dto);

            var expected = _readContext.Books.Single(_ => _.Id == actual);
            expected.MinAgeNeed.Should().Be(15);
            expected.Author.Should().Be("dummy-author");
            expected.BookCategoryId.Should().Be(bookCategory.Id);
            expected.Name.Should().Be("dummy-name");
            expected.Id.Should().Be(actual);
        }

        [Fact]
        public async void Update_update_book_properly()
        {
            BookCategory bookCategory = BookCategoryFactory.GenerateBookCategory();
            _context.Manipulate(_=>_.BookCategories.Add(bookCategory));
            Book book = new BookBuilder()
                .BuildBookWithCategoryId(bookCategory.Id)
                .Build();
            _context.Manipulate(_=>_.Books.Add(book));
            UpdateBookDto dto = BookDtoFactory.GenerateUpdateBookDto(bookCategory.Id,
                20, "new-author", book.Name);

            var actual = await _sut.UpdateBook(book.Id, dto);

            var expected = _readContext.Books.Single(_ => _.Id == actual);
            expected.Author.Should().Be("new-author");
            expected.MinAgeNeed.Should().Be(20);
        }

        [Fact]
        public async void Update_failed_update_when_book_not_exists()
        {
            BookCategory bookCategory = BookCategoryFactory.GenerateBookCategory();
            _context.Manipulate(_=>_.BookCategories.Add(bookCategory));
            UpdateBookDto dto = BookDtoFactory.GenerateUpdateBookDto(bookCategory.Id,
                20, "dummy-author", "dummy-name");

            Func<Task> expected = ()=> _sut.UpdateBook(1, dto);

            expected.Should().ThrowExactly<FailedBookUpdateWhenBookNotExistsException>();
        }

        [Fact]
        public async void Get_get_books_by_categoryid()
        {
            List<Book> books = new List<Book>();
            var bookCategory = BookCategoryFactory.GenerateBookCategory();
            _context.Manipulate(_=>_.BookCategories.Add(bookCategory));
            var firstBook = new BookBuilder()
                .BuildBookWithName("first-book")
                .BuildBookWithCategoryId(bookCategory.Id)
                .Build();
            var secondBook = new BookBuilder()
                .BuildBookWithName("second-book")
                .BuildBookWithCategoryId(bookCategory.Id)
                .Build(); ;
            books.Add(firstBook);
            books.Add(secondBook);
            _context.Manipulate(_=>_.Books.AddRange(books));

            var expected =await _sut.GetBooksByCategoryId(bookCategory.Id);

            expected.Should().HaveCount(2);
            expected.Select(_ => _.Name).Should().Contain("first-book");
            expected.Select(_ => _.Name).Should().Contain("second-book");
        }

        [Fact]
        public async void Get_find_book_by_id()
        {
            var bookCategory = BookCategoryFactory.GenerateBookCategory("dummy-title");
            _context.Manipulate(_=>_.BookCategories.Add(bookCategory));
            var book = new BookBuilder()
                .BuildBookWithCategoryId(bookCategory.Id)
                .BuildBookWithName("dummy-name")
                .Build();
            _context.Manipulate(_=>_.Books.Add(book));

            var expected = await _sut.FindBookById(book.Id);

            expected.BookCategoryId.Should().Be(bookCategory.Id);
            expected.Name.Should().Be("dummy-name");
        }
    }
}
