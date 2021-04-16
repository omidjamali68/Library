using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Library.Entities;
using Library.Services.Members.Contracts;

namespace Library.Persistence.EF.Members
{
    public class EFMemberRepository : MemberRepository
    {
        private readonly EFDataContext _context;

        public EFMemberRepository(EFDataContext context)
        {
            _context = context;
        }
        public void Add(Member member)
        {
            _context.Members.Add(member);
        }

        public Member FindMemberById(int id)
        {
            var member = _context.Members.Single(_ => _.Id == id);
            return member;
        }
    }
}
