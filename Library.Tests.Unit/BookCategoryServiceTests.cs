using System;
using System.Linq;
using FluentAssertions;
using Library.Entities;
using Library.Persistence.EF;
using Library.Persistence.EF.BookCategories;
using Library.Services.BookCategories;
using Library.Services.BookCategories.Contracts;
using Work.Infrastructure.Test;
using Xunit;

namespace Library.Tests.Unit
{
    public class BookCategoryServiceTests
    {

        [Fact]
        public async void Add_add_category_properly()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            var readContext = db.CreateDataContext<EFDataContext>();
            var repository=new EFBookCategoryRepository(context);
            var unitOfWork=new EFUnitOfWork(context);
            var category = new BookCategory { Title = "dummy-title"};
            var dto = new AddBookCategoryDto { Title = category.Title};
            var sut=new BookCategoryAppService(unitOfWork,repository);

            var actual=await sut.AddCategory(dto);

            var expected = readContext.BookCategories.Single(_=>_.Id==actual);
            expected.Title.Should().Be(dto.Title);
        }
    }
}
