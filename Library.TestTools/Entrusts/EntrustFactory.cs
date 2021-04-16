using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Library.Services.Entrusts.Contracts;

namespace Library.TestTools.Entrusts
{
    public static class EntrustFactory
    {
        public static AddEntrustDto GenerateAddEntrustDto(int bookId,int memberId,DateTime bookReturnDate)
        {
            return new AddEntrustDto
            {
                MemberId = memberId,
                BookReturnDate = bookReturnDate,
                BookId = bookId
            };
        }

        public static Entrust GenerateEntrust(int bookId,int memberId,DateTime expectReturnDate)
        {
            return new Entrust
            {
                BookId = bookId,
                MemberId = memberId,
                DeterminateReturnDate = expectReturnDate
            };
        }

        public static UpdateEntrustRealReturnDateDto GenerateEntrustRealReturnDateDto(DateTime realDate)
        {
            return new UpdateEntrustRealReturnDateDto
            {
                RealReturnDate = realDate
            };
        }
    }
}
