using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Library.Infrastructure.Application;

namespace Library.Services.Members.Contracts
{
    public interface MemberService : Service
    {
        Task<int> AddMember(AddMemberDto dto);
        Task<Member> FindMemberById(int id);
    }
}
