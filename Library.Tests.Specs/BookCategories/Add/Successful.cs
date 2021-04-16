using System.Linq;
using FluentAssertions;
using Library.Persistence.EF;
using Library.Persistence.EF.BookCategories;
using Library.Services.BookCategories;
using Library.TestTools.BookCategories;
using Work.Infrastructure.Test;
using Xunit;

namespace Library.Tests.Specs.BookCategories.Add
{
    public class Successful
    {
        private readonly BookCategoryAppService _sut;
        private readonly EFDataContext _readContext;
        private int _categoryId;

        public Successful()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository = new EFBookCategoryRepository(context);
            var unitOfWork = new EFUnitOfWork(context);
            _sut = new BookCategoryAppService(unitOfWork,repository); 
        }
        //هیچ دسته بندی در فهرست دسته بندی ها وجود ندارد
        private void Given()
        {

        }

        //یک دسته با عنوان شعر وادبیات در فهرست دسته بندی ها تعریف میکنم
        private async void When()
        {
            var bookCategory = BookCategoryFactory.GenerateBookCategory("شعروادبیات");
            var dto=BookCategoryFactory.GenerateAddBookCategoryDto(bookCategory.Title);
            _categoryId = await _sut.AddCategory(dto);
        }

        //باید فقط یک دسته با عنوان شعر وادبیات
        //در فهرست دسته بندی ها وجود داشته باشد
        private void Then()
        {
            var expected = _readContext.BookCategories
                .Single(_ => _.Id == _categoryId);
            expected.Title.Should().Be("شعروادبیات");
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
