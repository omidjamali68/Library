using System;
using System.Linq;
using FluentAssertions;
using Library.Entities;
using Library.Infrastructure.Application;
using Library.Persistence.EF;
using Library.Persistence.EF.Books;
using Library.Persistence.EF.Entrusts;
using Library.Persistence.EF.Members;
using Library.Services.Entrusts;
using Library.Services.Entrusts.Contracts;
using Library.TestTools.BookCategories;
using Library.TestTools.Books;
using Library.TestTools.Entrusts;
using Library.TestTools.Members;
using Work.Infrastructure.Test;
using Xunit;

namespace Library.Tests.Specs.Entrusts.Add
{
    public class Successful
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly EntrustService _sut;
        private Book _book;
        private Member _member;
        private int _entrustId;
        public Successful()
        {
            var db=new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository = new EFEntrustRepository(_context);
            var unitOfWork=new EFUnitOfWork(_context);
            var bookRepository=new EFBookRepository(_context);
            var memberRepository=new EFMemberRepository(_context);
            _sut = new EntrustAppService(repository, unitOfWork,bookRepository,memberRepository);
        }

        //یک دسته بندی با عنوان شعر و ادبیات در فهرست دسته بندی ها وجود دارد
        // و یک کتاب به نویسندگی فروغ فرخزاد با عنوان دیوان اشعار فروغ و
        // رده سنی بالای 15 سال در فهرست کتاب ها و در دسته شعر و ادبیات وجود دارد
        //و یک عضو با نام ونام خانوادگی
        //امید جمالی و سن 31 سال و آدرس بلوار محراب در فهرست اعضا وجود دارد
        //و هیچ امانتی در فهرست کتاب های امانت داده شده وجود ندارد
        private void Given()
        {
            var bookCategory = BookCategoryFactory.GenerateBookCategory("شعروادبیات");
            _context.Manipulate(_ => _.BookCategories.Add(bookCategory));

            _book = new BookBuilder().BuildBookWithName("دیوان اشعارفروغ")
                .BuildBookWithAuthor("فروغ فرخزاد")
                .BuildBookWithAgeRange(15)
                .BuildBookWithCategoryId(bookCategory.Id)
                .Build();
            _context.Manipulate(_=>_.Books.Add(_book));
            _member = MemberFactory.GenerateMember("بلوارمحراب", 31, "امیدجمالی");
            _context.Manipulate(_=>_.Members.Add(_member));
        }

        //کتاب با عنوان دیوان اشعار فروغ و رده سنی
        //بالای 15 سال به نوسندگی فروغ فرخزاد به عضوی با نام و نام خانوادگی امید جمالی
        //و سن 31 سال و آدرس بلوار محراب و تاریخ برگشت تعیین شده 30/02/1400 امانت داده میشود
        private async void When()
        {
            var dto = EntrustFactory.GenerateAddEntrustDto(_book.Id, _member.Id, DateTime.UtcNow.Date);
            _entrustId=await _sut.AddEntrust(dto);
        }

        //باید فقط یک امانت کتاب با عنوان دیوان اشعار فروغ و رده سنی بالای 15 سال
        //به نوسندگی فروغ فرخزاد برای عضوی با نام و نام خانوادگی امید جمالی و سن 31 سال و آدرس
        //بلوار محراب و تاریخ برگشت تعیین شده 30/02/1400 در فهرست کتاب های امانت داده شده وجو داشته باشد .
        private void Then()
        {
            var expected = _readContext.Entrusts.Single(_ => _.Id == _entrustId);
            expected.BookId.Should().Be(_book.Id);
            expected.MemberId.Should().Be(_member.Id);
            expected.DeterminateReturnDate.Should().Be(DateTime.UtcNow.Date);
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
