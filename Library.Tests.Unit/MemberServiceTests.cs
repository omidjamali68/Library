using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Library.Persistence.EF;
using Library.Persistence.EF.Members;
using Library.Services.Members;
using Library.Services.Members.Contracts;
using Library.TestTools.Members;
using Work.Infrastructure.Test;
using Xunit;

namespace Library.Tests.Unit
{
    public class MemberServiceTests
    {
        private readonly EFDataContext _readContext;
        private readonly EFDataContext _context;
        private readonly MemberService _sut;
        public MemberServiceTests()
        {
            var db = new EFInMemoryDatabase();
            _context = db.CreateDataContext<EFDataContext>();
            _readContext = db.CreateDataContext<EFDataContext>();
            var repository = new EFMemberRepository(_context);
            var unitOfWork = new EFUnitOfWork(_context);
            _sut = new MemberAppService(repository, unitOfWork);
        }

        [Fact]
        public async void Add_add_member_properly()
        {
            
            var dto = MemberFactory.GenerateAddMemberDto("dummy-address",
                31, "dummy-fullname");

            var actual = await _sut.AddMember(dto);

            var expected = _readContext.Members.Single(_ => _.Id == actual);
            expected.FullName.Should().Be("dummy-fullname");
            expected.Age.Should().Be(31);
            expected.Address.Should().Be("dummy-address");
            expected.Id.Should().Be(actual);
        }

        [Fact]
        public async void Get_find_member_by_member_id()
        {
            var member = MemberFactory.GenerateMember("dummy-address", 31, "dummy-fullname");
            _context.Manipulate(_=>_.Add(member));

            var expected = await _sut.FindMemberById(member.Id);

            expected.Age.Should().Be(31);
            expected.FullName.Should().Be("dummy-fullname");
            expected.Address.Should().Be("dummy-address");
        }
    }
}
