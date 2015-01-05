using System;
using System.Data.Entity;

namespace DAL.Interface.Repository
{
    public interface IUnitOfWork: IDisposable
    {
        DbContext Context { get; }
        void Commit();
    }
}
