using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Library.Infrastructure.Application;
using Library.Services.Members.Contracts;

namespace Library.Services.Members
{
    public class MemberAppService : MemberService
    {
        private readonly MemberRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public MemberAppService(MemberRepository repository, UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> AddMember(AddMemberDto dto)
        {
            Member member=new Member
            {
                Address = dto.Address,
                Age = dto.Age,
                FullName = dto.FullName
            };
            _repository.Add(member);
            await _unitOfWork.Complete();
            return member.Id;
        }

        public async Task<Member> FindMemberById(int id)
        {
           return await Task.Run(() => 
                _repository.FindMemberById(id)
                );
        }
    }
}
