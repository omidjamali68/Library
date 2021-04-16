using System;
using System.Collections.Generic;
using System.Text;
using Library.Entities;
using Library.Services.Entrusts.Contracts;

namespace Library.Persistence.EF.Entrusts
{
    public class EFEntrustRepository : EntrustRepository
    {
        private readonly EFDataContext _context;

        public EFEntrustRepository(EFDataContext context)
        {
            _context = context;
        }

        public void Add(Entrust entrust)
        {
            _context.Entrusts.Add(entrust);
        }

        public Entrust FindById(int id)
        {
            return _context.Entrusts.Find(id);
        }

        public Book FindBookById(int id)
        {
            return _context.Books.Find(id);
        }
    }
}
