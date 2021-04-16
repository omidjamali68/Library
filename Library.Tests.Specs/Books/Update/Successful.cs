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

namespace Library.Tests.Specs.Books.Update
{
    public class Successful
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly BookService _sut;
        private BookCategory _bookCategory;
        private Book _book;

        public Successful()
        {
            var db=new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository=new EFBookRepository(_context);
            var unitOfWork=new EFUnitOfWork(_context);
            _sut=new BookAppService(repository,unitOfWork);
        }

        //یک دسته بندی با عنوان شعر و ادبیات در فهرست دسته بندی ها وجود دارد
        //و فقط یک کتاب به نویسندگی فروغ فرخزاد با عنوان دیوان اشعار فروغ و
        //رنج سنی بالای 15 سال در فهرست کتاب ها و در دسته شعر و ادبیات وجود دارد
        private void Given()
        {
            _bookCategory = BookCategoryFactory.GenerateBookCategory("شعروادبیات");
            _context.Manipulate(_=>_.BookCategories.Add(_bookCategory));

            _book = new BookBuilder().BuildBookWithName("دیوان اشعارفروغ")
                .BuildBookWithAuthor("فروغ فرخزاد")
                .BuildBookWithAgeRange(15)
                .BuildBookWithCategoryId(_bookCategory.Id)
                .Build();
            _context.Manipulate(_=>_.Books.Add(_book));
        }

        //مشخصات یک کتاب به نویسندگی فروغ فرخزاد
        //با عنوان دیوان اشعار فروغ و رنج سنی بالای 15 سال در دسته
        //شعر و ادبیات را به کتاب به نویسندگی فروغ فرخزاد با عنوان  اشعار
        //فروغ  و رنج سنی بالای 20 سال در دسته شعر وادبیات تغییر میدهم
        private void When()
        {
            var updateDto = BookDtoFactory.GenerateUpdateBookDto(_bookCategory.Id,
                20, "فروغ فرخزاد", "اشعارفروغ");
            _sut.UpdateBook(_book.Id,updateDto);
        }

        //باید فقط یک کتاب به نویسندگی فروغ فرخزاد با عنوان اشعار فروغ  و
        //رنج سنی بالای 20 سال در فهرست کتاب ها و دسته شعر وادبیات وجود داشته باشد
        private void Then()
        {
            var expected = _readContext.Books.Single(_ => _.Id == _book.Id);
            expected.Name.Should().Be("اشعارفروغ");
            expected.MinAgeNeed.Should().Be(20);
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
