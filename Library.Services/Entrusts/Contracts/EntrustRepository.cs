using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Library.Infrastructure.Application;

namespace Library.Services.Entrusts.Contracts
{
    public interface EntrustRepository : Repository
    {
        void Add(Entrust entrust);
        Entrust FindById(int id);
        Book FindBookById(int id);
    }
}
