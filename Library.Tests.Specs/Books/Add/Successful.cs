using System.Linq;
using FluentAssertions;
using Library.Entities;
using Library.Persistence.EF;
using Library.Persistence.EF.Books;
using Library.Services.Books;
using Library.Services.Books.Contracts;
using Library.TestTools.BookCategories;
using Library.TestTools.Books;
using Work.Infrastructure.Test;
using Xunit;

namespace Library.Tests.Specs.Books.Add
{
    public class Successful
    {
        private readonly BookService _sut;
        private readonly EFDataContext _readContext;
        private readonly EFDataContext _context;
        private BookCategory _bookCategory;
        private int _bookId;

        public Successful()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository = new EFBookRepository(_context);
            var unitOfWork = new EFUnitOfWork(_context);
            _sut = new BookAppService(repository, unitOfWork);
        }

        //یک دسته بندی با عنوان شعر و ادبیات در فهرست دسته بندی ها وجود دارد
        private void Given()
        {
            _bookCategory = BookCategoryFactory.GenerateBookCategory("شعروادبیات");
            _context.Manipulate(_=>_.BookCategories.Add(_bookCategory));
        }

        //یک کتاب به نویسندگی فروغ فرخزاد با عنوان دیوان اشعار فروغ
        //و رنج سنی بالای 15 سال در فهرست کتاب ها و در دسته شعر و ادبیات تعریف میکنم
        private async void When()
        {
            var addDto = BookDtoFactory.GenerateAddBookDto(_bookCategory.Id
                , 15
                , "فروغ فرخزاد", "دیوان اشعارفروغ");
            _bookId = await _sut.AddBook(addDto);
        }

        //باید فقط یک کتاب به نویسندگی فروغ فرخزاد با عنوان دیوان اشعار فروغ
        //و رنج سنی بالای 15 سال در  فهرست کتاب ها و دسته شعر وادبیات وجود داشته باشد.
        private void Then()
        {
            var expected = _readContext.Books.Single(_ => _.Id == _bookId);
            expected.Author.Should().Be("فروغ فرخزاد");
            expected.Name.Should().Be("دیوان اشعارفروغ");
            expected.BookCategoryId.Should().Be(_bookCategory.Id);
            expected.MinAgeNeed.Should().Be(15);
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
    }
}
