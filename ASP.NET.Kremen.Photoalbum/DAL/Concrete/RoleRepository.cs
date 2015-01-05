using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using DAL.Interface.DTO;
using DAL.Interface.Repository;
using ORM;

namespace DAL.Concrete
{
    public class RoleRepository : IRoleRepository
    {
         private readonly DbContext context;

         public RoleRepository(IUnitOfWork uow)
        {
            this.context = uow.Context;
            Debug.WriteLine("RoleRepository create!");
        }

        public IEnumerable<DalRole> GetAll()
        {
            return context.Set<Role>().Select(role => new DalRole()
            {
                Id = role.Id,
                RoleName = role.RoleName
            });
        }

        public DalRole GetById(int key)
        {
            var ormrole = context.Set<Role>().FirstOrDefault(role => role.Id == key);
            return new DalRole()
            {
                Id = ormrole.Id,
                RoleName = ormrole.RoleName

            };
        }

        public DalRole GetByPredicate(System.Linq.Expressions.Expression<Func<DalRole, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DalRole e)
        {
            var role = new Role()
            {
                RoleName = e.RoleName,
                Id = e.Id
            };
            context.Set<Role>().Add(role);
        }

        public void Delete(DalRole e)
        {
            var ormrole = context.Set<Role>().FirstOrDefault(role => role.Id == e.Id);
            if (ormrole != null)
            {
                context.Set<Role>().Remove(ormrole);
            }
        }

        public void Update(DalRole entity)
        {
            throw new NotImplementedException();
        }
    }
}
