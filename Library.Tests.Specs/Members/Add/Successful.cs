using System.Linq;
using FluentAssertions;
using Library.Persistence.EF;
using Library.Persistence.EF.Members;
using Library.Services.Members;
using Library.Services.Members.Contracts;
using Library.TestTools.Members;
using Work.Infrastructure.Test;
using Xunit;

namespace Library.Tests.Specs.Members.Add
{
    public class Successful
    {
        private readonly MemberService _sut;
        private readonly EFDataContext _readContext;
        private int _memberId;
        public Successful()
        {
            var db = new EFInMemoryDatabase();
            var context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository = new EFMemberRepository(context);
            var unitOfWork = new EFUnitOfWork(context);
            _sut = new MemberAppService(repository, unitOfWork);
        }

        //هیچ عضوی در فهرست اعضا وجود ندارد
        private void Given()
        {

        }

        //یک عضو با نام ونام خانوادگی امید جمالی
        //و سن 31 سال و آدرس بلوار محراب در فهرست اعضا تعریف میکنم
        private async void When()
        {
            var dto = MemberFactory.GenerateAddMemberDto("بلوارمحراب", 31, "امیدجمالی");
            _memberId = await _sut.AddMember(dto);
        }

        //باید فقط یک عضو با نام و نام خانوادگی
        //امید جمالی و سن 31 سال وآدرس بلوار محراب در فهرست اعضا وجود داشته باشد
        private void Then()
        {
            var expected = _readContext.Members.Single(_ => _.Id == _memberId);
            expected.FullName.Should().Be("امیدجمالی");
            expected.Age.Should().Be(31);
            expected.Address.Should().Be("بلوارمحراب");
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
