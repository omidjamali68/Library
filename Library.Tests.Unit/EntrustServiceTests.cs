using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

namespace Library.Tests.Unit
{
    public class EntrustServiceTests
    {
        private readonly EFDataContext _context;
        private readonly EFDataContext _readContext;
        private readonly EntrustService _sut;
        private readonly BookCategory _category;
        public EntrustServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository = new EFEntrustRepository(_context);
            var unitOfWork = new EFUnitOfWork(_context);
            var bookRepository=new EFBookRepository(_context);
            var memberRepository=new EFMemberRepository(_context);
            _sut = new EntrustAppService(repository, unitOfWork,bookRepository,memberRepository);
            _category= BookCategoryFactory.GenerateBookCategory("dummy-title");
        }

        [Fact]
        public async void Add_add_entrust_properly()
        {
            
            _context.Manipulate(_=>_.BookCategories.Add(_category));
            var book = new BookBuilder()
                .BuildBookWithCategoryId(_category.Id)
                .Build();
            _context.Manipulate(_=>_.Books.Add(book));
            var member = MemberFactory.GenerateMember("dummy-address", 31, "dummy-fulname");
            _context.Manipulate(_=>_.Members.Add(member));
            var dto = EntrustFactory.GenerateAddEntrustDto(book.Id, member.Id, DateTime.Today);

            var actual = await _sut.AddEntrust(dto);

            var expected = _readContext.Entrusts.Single(_ => _.Id == actual);
            expected.BookId.Should().Be(book.Id);
            expected.MemberId.Should().Be(member.Id);
            expected.DeterminateReturnDate.Should().Be(DateTime.Today);
        }

        [Fact]
        public void Add_failed_when_member_age_not_in_valid_range()
        {
            _context.Manipulate(_ => _.BookCategories.Add(_category));
            var book = new BookBuilder()
                .BuildBookWithCategoryId(_category.Id)
                .BuildBookWithAgeRange(15)
                .Build();
            _context.Manipulate(_=>_.Books.Add(book));
            var member = MemberFactory.GenerateMember("dummy-address", 11, "dummy-name");
            _context.Manipulate(_=>_.Members.Add(member));
            var dto = EntrustFactory.GenerateAddEntrustDto(book.Id, member.Id, DateTime.Today);

            Func<Task> expected = () => _sut.AddEntrust(dto);

            expected.Should().ThrowExactly<FailedAddEntrustWhenMemberAgeIsNotInValidRanegException>();
        }

        [Fact]
        public async void Get_find_entrust_by_id()
        {
            _context.Manipulate(_ => _.BookCategories.Add(_category));
            var book = new BookBuilder()
                .BuildBookWithCategoryId(_category.Id)
                .BuildBookWithAgeRange(15)
                .Build();
            _context.Manipulate(_ => _.Books.Add(book));
            var member = MemberFactory.GenerateMember("dummy-address", 31, "dummy-name");
            _context.Manipulate(_ => _.Members.Add(member));
            var entrust = EntrustFactory.GenerateEntrust(book.Id, member.Id, DateTime.Today);
            _context.Manipulate(_=>_.Entrusts.Add(entrust));

            var expected =await _sut.FindEntrustById(entrust.Id);

            expected.BookId.Should().Be(book.Id);
            expected.MemberId.Should().Be(member.Id);
            expected.DeterminateReturnDate.Should().Be(DateTime.Today);
        }

        [Fact]
        public async void Update_update_real_return_date_properly()
        {
            _context.Manipulate(_ => _.BookCategories.Add(_category));
            var book = new BookBuilder()
                .BuildBookWithCategoryId(_category.Id)
                .BuildBookWithAgeRange(15)
                .Build();
            _context.Manipulate(_ => _.Books.Add(book));
            var member = MemberFactory.GenerateMember("dummy-address", 31, "dummy-name");
            _context.Manipulate(_ => _.Members.Add(member));
            var determinateShamsiToMiladi=new DateTime(1400,2,30,new PersianCalendar());
            var entrust = EntrustFactory.GenerateEntrust(book.Id, member.Id, determinateShamsiToMiladi);
            _context.Manipulate(_ => _.Entrusts.Add(entrust));
            var realShamsiToMiladiDate=new DateTime(1400,2,25,new PersianCalendar());
            var dto = EntrustFactory.GenerateEntrustRealReturnDateDto(realShamsiToMiladiDate);

            var actual =await _sut.UpdateEntrustRealReturnDate(entrust.Id, dto);

            var expected = _readContext.Entrusts.Single(_ => _.Id == actual);
            expected.RealReturnDate.Should().Be(realShamsiToMiladiDate);
        }

        [Fact]
        public void Update_failed_when_determinate_return_date_was_expired()
        {
            _context.Manipulate(_ => _.BookCategories.Add(_category));
            var book = new BookBuilder()
                .BuildBookWithCategoryId(_category.Id)
                .BuildBookWithAgeRange(15)
                .Build();
            _context.Manipulate(_ => _.Books.Add(book));
            var member = MemberFactory.GenerateMember("dummy-address", 31, "dummy-name");
            _context.Manipulate(_ => _.Members.Add(member));
            var determinateShamsiToMiladi = new DateTime(1400, 2, 30, new PersianCalendar());
            var entrust = EntrustFactory.GenerateEntrust(book.Id, member.Id, determinateShamsiToMiladi);
            _context.Manipulate(_ => _.Entrusts.Add(entrust));
            var realShamsiToMiladiDate = new DateTime(1400, 3, 5, new PersianCalendar());
            var dto = EntrustFactory.GenerateEntrustRealReturnDateDto(realShamsiToMiladiDate);

            Func<Task> expected = () => _sut.UpdateEntrustRealReturnDate(entrust.Id,dto);

            expected.Should().ThrowExactly<FailedWhenDeterminateReturnDateWasExpiredException>();

        }
    }
}
