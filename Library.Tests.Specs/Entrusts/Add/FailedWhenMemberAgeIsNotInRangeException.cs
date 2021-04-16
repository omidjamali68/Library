using System;
using System.Threading.Tasks;
using FluentAssertions;
using Library.Entities;
using Library.Persistence.EF;
using Library.Persistence.EF.Books;
using Library.Persistence.EF.Entrusts;
using Library.Persistence.EF.Members;
using Library.Services.Entrusts;
using Library.Services.Entrusts.Contracts;
using Library.Services.Entrusts.Exceptions;
using Library.TestTools.BookCategories;
using Library.TestTools.Books;
using Library.TestTools.Entrusts;
using Library.TestTools.Members;
using Work.Infrastructure.Test;
using Xunit;

namespace Library.Tests.Specs.Entrusts.Add
{
    public class FailedWhenMemberAgeIsNotInRangeException
    {
        private readonly EntrustService _sut;
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private Book _book;
        private Member _member;
        private Func<Task> _expected;

        public FailedWhenMemberAgeIsNotInRangeException()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository = new EFEntrustRepository(_context);
            var unitOfWork = new EFUnitOfWork(_context);
            var bookRepository=new EFBookRepository(_context);
            var memberRepository=new EFMemberRepository(_context);
            _sut = new EntrustAppService(repository, unitOfWork,bookRepository,memberRepository);
        }

        //یک دسته بندی با عنوان شعر و ادبیات در فهرست دسته بندی ها وجود دارد
        //و یک کتاب به نویسندگی فروغ فرخزاد با عنوان دیوان
        //اشعار فروغ و رده سنی بالای 15 سال در فهرست کتاب ها و در دسته شعر و ادبیات وجود دارد
        //و یک عضو با نام ونام خانوادگی بردیا جمالی و سن 11 سال و آدرس شیراز ، بلوار محراب در فهرست اعضا وجود دارد
        //و هیچ امانتی در فهرست کتاب های امانت داده شده وجود ندارد
        private void Given()
        {
            
            var bookCategory = BookCategoryFactory.GenerateBookCategory("شعروادبیات");
            _context.Manipulate(_=>_.BookCategories.Add(bookCategory));
            _book = new BookBuilder()
                .BuildBookWithCategoryId(bookCategory.Id)
                .BuildBookWithAgeRange(15)
                .BuildBookWithAuthor("فروغ فرخزاد")
                .BuildBookWithName("دیوان اشعارفروغ")
                .Build();
            _context.Manipulate(_=>_.Books.Add(_book));
            _member = MemberFactory.GenerateMember("بلوارمحراب", 11, "امیدجمالی");
            _context.Manipulate(_=>_.Members.Add(_member));
        }

        //کتاب با عنوان دیوان اشعار فروغ و رده سنی بالای 15 سال به
        //عضوی با نام و نام خانوادگی بردیا جمالی و سن 11 سال امانت داده میشود
        private void When()
        {
            var dto = EntrustFactory.GenerateAddEntrustDto(_book.Id, _member.Id, DateTime.Today);
            _expected = ()=> _sut.AddEntrust(dto);
        }

        //باید خطای سن فرد برای امانت بردن این کتاب کافی نیست رخ دهد
        //و نباید هیچ امانتی در فهرست کتاب های امانت داده شده وجود داشته باشد . 
        private void Then()
        {
            _expected.Should().ThrowExactly<FailedAddEntrustWhenMemberAgeIsNotInValidRanegException>();
            var expectedEntrust = _readContext.Entrusts;
            expectedEntrust.Should().HaveCount(0);
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
