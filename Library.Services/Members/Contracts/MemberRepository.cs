using Library.Entities;
using Library.Infrastructure.Application;

namespace Library.Services.Members.Contracts
{
    public interface MemberRepository : Repository
    {
        void Add(Member member);
        Member FindMemberById(int id);
    }
}
