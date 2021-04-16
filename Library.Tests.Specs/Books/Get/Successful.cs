using System.Collections.Generic;
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

namespace Library.Tests.Specs.Books.Get
{
    public class Successful
    {
        private readonly EFDataContext _context;
        private readonly BookService _sut;
        private BookCategory _bookCategory;
        private IEnumerable<Book> _expected;


        public Successful()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            var repository = new EFBookRepository(_context);
            var unitOfWork = new EFUnitOfWork(_context);
            _sut = new BookAppService(repository, unitOfWork);
        }

        //یک دسته بندی با عنوان شعر و ادبیات در فهرست دسته بندی ها وجود دارد
        //و به تعداد 3 کتاب با عنوان های دیوان فروغ ،
        //دیوان حافظ و گلستان سعدی در فهرست کتاب ها در دسته شعر وادبیات تعریف شده است
        private void Given()
        {
            _bookCategory = BookCategoryFactory.GenerateBookCategory("شعروادبیات");
            _context.Manipulate(_=>_.BookCategories.Add(_bookCategory));
            var bookFrogh=new BookBuilder()
                .BuildBookWithName("دیوان فروغ")
                .BuildBookWithCategoryId(_bookCategory.Id)
                .Build();
            var bookHafez= new BookBuilder()
                .BuildBookWithName("دیوان حافظ")
                .BuildBookWithCategoryId(_bookCategory.Id)
                .Build();
            var bookSaadi= new BookBuilder()
                .BuildBookWithName("گلستان سعدی")
                .BuildBookWithCategoryId(_bookCategory.Id)
                .Build();
            _context.Manipulate(_ => _.Books.Add(bookFrogh));
            _context.Manipulate(_ => _.Books.Add(bookHafez));
            _context.Manipulate(_ => _.Books.Add(bookSaadi));
        }

        //فهرست کتاب های تعریف شده در دسته شعر و ادبیات را مشاهده میکنم
        private async void When()
        {
            _expected=await _sut.GetBooksByCategoryId(_bookCategory.Id);
        }

        //باید به تعداد 3 کتاب با عنوان های دیوان فروغ ، دیوان حافظ
        //و گلستان سعدی در دسته بندی شعر و ادبیات در فهرست کتاب ها وجود داشته باشد
        private void Then()
        {
            _expected.Should().HaveCount(3);
            _expected.Select(_ => _.Name).Should().Contain("دیوان فروغ");
            _expected.Select(_ => _.Name).Should().Contain("دیوان حافظ");
            _expected.Select(_ => _.Name).Should().Contain("گلستان سعدی");
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
