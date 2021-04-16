using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Library.Infrastructure.Application;
using Library.Services.Books.Contracts;
using Library.Services.Entrusts.Contracts;
using Library.Services.Entrusts.Exceptions;
using Library.Services.Members.Contracts;

namespace Library.Services.Entrusts
{
    public class EntrustAppService : EntrustService
    {
        private readonly EntrustRepository _entrustRepository;
        private readonly UnitOfWork _unitOfWork;
        private readonly BookRepository _bookRepository;
        private readonly MemberRepository _memberRepository;

        public EntrustAppService(EntrustRepository entrustRepository
            , UnitOfWork unitOfWork,BookRepository bookRepository,MemberRepository memberRepository)
        {
            _entrustRepository = entrustRepository;
            _unitOfWork = unitOfWork;
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
        }

        public async Task<int> AddEntrust(AddEntrustDto dto)
        {
            var book = _entrustRepository.FindBookById(dto.BookId);
            var member = _memberRepository.FindMemberById(dto.MemberId);
            short bookMinAgeNeeded = (short) book.MinAgeNeed;
            if (member.Age < bookMinAgeNeeded)
            {
                throw new FailedAddEntrustWhenMemberAgeIsNotInValidRanegException();
            }
            Entrust entrust = new Entrust
            {
                BookId = dto.BookId,
                MemberId = dto.MemberId,
                DeterminateReturnDate = dto.BookReturnDate
            };
            _entrustRepository.Add(entrust);
            await _unitOfWork.Complete();
            return entrust.Id;
        }

        public async Task<int> UpdateEntrustRealReturnDate(int id, UpdateEntrustRealReturnDateDto dto)
        {
            var existsEntrust = _entrustRepository.FindById(id);
            if (dto.RealReturnDate > existsEntrust.DeterminateReturnDate)
            {
                throw new FailedWhenDeterminateReturnDateWasExpiredException();
            }
            existsEntrust.RealReturnDate = dto.RealReturnDate;
            await _unitOfWork.Complete();
            return existsEntrust.Id;
        }

        public async Task<Entrust> FindEntrustById(int id)
        {
            return await Task.Run(() => 
                _entrustRepository.FindById(id)
            );
        }
    }
}
