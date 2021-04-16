using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.Entities;
using Library.Infrastructure.Application;

namespace Library.Services.Entrusts.Contracts
{
    public interface EntrustService : Service
    {
        Task<int> AddEntrust(AddEntrustDto dto);
        Task<int> UpdateEntrustRealReturnDate(int id,UpdateEntrustRealReturnDateDto dto);

        Task<Entrust> FindEntrustById(int id);
    }
}
