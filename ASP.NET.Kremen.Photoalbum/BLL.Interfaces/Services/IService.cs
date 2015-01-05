using System.Collections.Generic;

namespace BLL.Interfaces.Services
{
    public interface IService<TEntity>
    {
         IEnumerable<TEntity> GetAll();
        void Create(TEntity entity);
        void Delete(TEntity entity);
        TEntity GetById(int key);
    }
}
