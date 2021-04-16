using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FluentAssertions;
using Library.Entities;
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

namespace Library.Tests.Specs.Entrusts.Update
{
    public class Successful
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly EntrustService _sut;
        private Entrust _entrust;
        private int _entrustId;
        private DateTime _determinateShamsiToMiladiDate;
        private DateTime _realShamsiToMiladiDate;
        public Successful()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository=new EFEntrustRepository(_context);
            var unitOfWork=new EFUnitOfWork(_context);
            var bookRepository=new EFBookRepository(_context);
            var memberRepository=new EFMemberRepository(_context);
            _sut=new EntrustAppService(repository,unitOfWork,bookRepository,memberRepository);
        }

        //یک دسته بندی با عنوان شعر و ادبیات در فهرست دسته بندی ها وجود دارد
        //و یک کتاب به نویسندگی فروغ فرخزاد با عنوان دیوان اشعار فروغ و رده سنی بالای 15 سال در فهرست کتاب ها و در دسته شعر و ادبیات وجود دارد
        //و یک عضو با نام ونام خانوادگی امید جمالی و سن 31 سال و آدرس بلوار محراب در فهرست اعضا وجود دارد
        //و فقط یک امانت کتاب با عنوان دیوان اشعار فروغ به نویسندگی
        //فروغ فرخزاد و رده سنی بالای 15 سال برای فردی با نام و نام خانوادگی امید جمالی و سن 31 سال
        //و آدرس بلوار محراب و تاریخ برگشت مورد انتظار30/02/1400 در فهرست کتاب های امانت داده شده وجود دارد.
        private void Given()
        {
            
            var bookCategory = BookCategoryFactory.GenerateBookCategory("شعروادبیات");
            _context.Manipulate(_=>_.BookCategories.Add(bookCategory));
            var book = new BookBuilder()
                .BuildBookWithCategoryId(bookCategory.Id)
                .BuildBookWithAgeRange(15)
                .BuildBookWithAuthor("فروغ فرخزاد")
                .BuildBookWithName("دیوان اشعارفروغ")
                .Build();
            _context.Manipulate(_=>_.Books.Add(book));
            var member = MemberFactory.GenerateMember("بلوارمحراب", 31, "امیدجمالی");
            _context.Manipulate(_=>_.Add(member));
            _determinateShamsiToMiladiDate=new DateTime(1400,2,30,new PersianCalendar());
            _entrust = EntrustFactory.GenerateEntrust(book.Id,member.Id,_determinateShamsiToMiladiDate);
            _context.Manipulate(_=>_.Entrusts.Add(_entrust));
        }

        //فردی با نام ونام خانوادگی امید جمالی و سن 31 سال و آدرس بلوار محراب در تاریخ 25/02/1400 کتاب
        //با عنوان دیوان اشعار فروغ و تاریخ برگشت تعیین شده 30/02/1400 را تحویل میدهد
        private async void When()
        {
            _realShamsiToMiladiDate=new DateTime(1400,2,25,new PersianCalendar());
            var dto=EntrustFactory.GenerateEntrustRealReturnDateDto(_realShamsiToMiladiDate);
            _entrustId = await _sut.UpdateEntrustRealReturnDate(_entrust.Id,dto);
        }

        //باید فقط یک امانت کتاب با عنوان دیوان اشعار فروغ به نویسندگی فروغ فرخزاد و رده سنی بالای 15 سال
        //برای فردی با نام و نام خانوادگی امید جمالی وسن 31 سال وآدرس بلوارمحراب و تاریخ برگشت مورد
        //انتظار 30/02/1400 و تاریخ برگشت واقعی 25/02/1400 در فهرست کتاب های امانت داده شده ثبت شود .
        private void Then()
        {
            var expected = _readContext.Entrusts.Single(_ => _.Id == _entrustId);
            expected.RealReturnDate.Should().Be(_realShamsiToMiladiDate);
            expected.DeterminateReturnDate.Should().Be(_determinateShamsiToMiladiDate);
            expected.Id.Should().Be(_entrust.Id);
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
