using System.Threading.Tasks;
using Library.InfraStructure;
using Library.Infrastructure.Application;

namespace Library.Persistence.EF
{
    public class EFUnitOfWork : UnitOfWork 
    {
        private readonly EFDataContext _dataContext;

        public EFUnitOfWork(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Begin()
        {
            _dataContext.Database.BeginTransaction();
        }

        public async Task CommitPartial()
        {
            await _dataContext.SaveChangesAsync();
        }

        public void Commit()
        {
            _dataContext.SaveChanges();
            _dataContext.Database.CommitTransaction();
        }

        public void Rollback()
        {
            _dataContext.Database.RollbackTransaction();
        }

        public async Task Complete()
        {
            await _dataContext.SaveChangesAsync();
        }
    }
}
