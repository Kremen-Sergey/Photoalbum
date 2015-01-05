using System;
using System.Data.Entity;
using System.Diagnostics;
using DAL.Interface.Repository;

namespace DAL.Concrete
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; private set; }

        public UnitOfWork(DbContext context)
        {
            Context = context;
            Debug.WriteLine("unit of work create!");
        }

        public void Commit()
        {
            if (Context != null)
            {
                Context.SaveChanges();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            Debug.WriteLine("Context dispose!");
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }
}
