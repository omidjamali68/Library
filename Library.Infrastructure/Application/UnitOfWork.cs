using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Library.InfraStructure;

namespace Library.Infrastructure.Application
{
    public interface UnitOfWork 
    {
        void Begin();
        void Commit();
        void Rollback();
        Task CommitPartial();
        Task Complete();

    }
}
