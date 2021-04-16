using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
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

namespace Library.Tests.Specs.Entrusts.Update
{
    public class FailedWhenDeterminateReturnDateWasExpired
    {
        private readonly EFDataContext _context;
        private Entrust _entrust;
        private readonly EntrustService _sut;
        private Func<Task> _expected;
        public FailedWhenDeterminateReturnDateWasExpired()
        {
            var db=new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            var repository=new EFEntrustRepository(_context);
            var unitOfWork=new EFUnitOfWork(_context);
            var bookRepository=new EFBookRepository(_context);
            var memberRepository=new EFMemberRepository(_context);
            _sut=new EntrustAppService(repository,unitOfWork,bookRepository,memberRepository);
        }

        //یک دسته بندی با عنوان شعر و ادبیات در فهرست دسته بندی ها وجود دارد
        //و یک کتاب به نویسندگی فروغ فرخزاد با عنوان دیوان اشعار فروغ و رده سنی بالای 15 سال در فهرست کتاب ها و در دسته شعر و ادبیات وجود دارد
        //و یک عضو با نام ونام خانوادگی امید جمالی و سن 31 سال و آدرس بلوار محراب در فهرست اعضا وجود دارد
        //و فقط یک امانت کتاب با عنوان دیوان اشعار فروغ به نویسندگی فروغ
        //فرخزاد و رده سنی بالای 15 سال برای فردی با نام و نام خانوادگی امید جمالی و سن 31 سال و آدرس
        //بلوار محراب و تاریخ برگشت تعیین شده 30/02/1400 در فهرست کتاب های امانت داده شده وجود دارد.
        private void Given()
        {
            var category = BookCategoryFactory.GenerateBookCategory("شعروادبیات");
            _context.Manipulate(_=>_.Add(category));
            var book = new BookBuilder()
                .BuildBookWithCategoryId(category.Id)
                .BuildBookWithAgeRange(15)
                .BuildBookWithAuthor("فروغ فرخزاد")
                .BuildBookWithName("دیوان اشعارفروغ")
                .Build();
            _context.Manipulate(_ => _.Books.Add(book));
            var member = MemberFactory.GenerateMember("بلوارمحراب", 31, "امیدجمالی");
            _context.Manipulate(_ => _.Add(member));
            var shamsiToMiladiDate = new DateTime(1400, 2, 30,new PersianCalendar());
            _entrust = EntrustFactory.GenerateEntrust(book.Id, member.Id, shamsiToMiladiDate);
            _context.Manipulate(_ => _.Entrusts.Add(_entrust));
        }

        //فردی با نام ونام خانوادگی امید جمالی و سن 31 سال و آدرس بلوار محراب در
        //تاریخ05/03/1400 کتاب با عنوان دیوان اشعار فروغ و تاریخ برگشت تعیین شده 30/02/1400 را تحویل میدهد
        private async void When()
        {
            var realDateShamsiToMiladi= new DateTime(1400, 3, 5, new PersianCalendar());
            var dto = EntrustFactory.GenerateEntrustRealReturnDateDto(realDateShamsiToMiladi);
            _expected = () =>  _sut.UpdateEntrustRealReturnDate(_entrust.Id, dto);
        }

        //باید خطای تاریخ برگشت مورد انتظار منقضی شده است رخ دهد .
        private void Then()
        {
            _expected.Should().ThrowExactly<FailedWhenDeterminateReturnDateWasExpiredException>();
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
