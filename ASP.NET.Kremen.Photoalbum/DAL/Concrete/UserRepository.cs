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
    public class UserRepository : IUserRepository
    {
        private readonly DbContext context;

        public UserRepository(IUnitOfWork uow)
        {
            this.context = uow.Context;
            Debug.WriteLine("UserRepository create!");
        }

        public IEnumerable<DalUser> GetAll()
        {
            return context.Set<User>().Select(user => new DalUser()
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Password = user.Password,
                UserPhotoe = user.UserPhotoe,
                UserPhotoMimeType = user.UserPhotoMimeType,
                CreationTime = user.CreationTime,
                LastUpdateTime = user.LastUpdateTime,
                RoleId = user.RoleId,
                Role = user.Role
            });
        }

        public DalUser GetById(int key)
        {
            var ormuser = context.Set<User>().FirstOrDefault(user => user.Id == key);
            if (ormuser == null) {return null;}
            return new DalUser()
            {
                Id = ormuser.Id,
                UserName = ormuser.UserName,
                RoleId = ormuser.RoleId,
                Email = ormuser.Email,
                Password = ormuser.Password,
                UserPhotoe = ormuser.UserPhotoe,
                UserPhotoMimeType = ormuser.UserPhotoMimeType,
                CreationTime = ormuser.CreationTime,
                LastUpdateTime = ormuser.LastUpdateTime,
                Role = ormuser.Role

            };
        }

        public DalUser GetByPredicate(System.Linq.Expressions.Expression<Func<DalUser, bool>> f)
        {
            throw new NotImplementedException();
        }

        public void Create(DalUser e)
        {
            var user = new User()
            {
                Id = e.Id,
                UserName = e.UserName,
                Email = e.Email,
                Password = e.Password,
                UserPhotoe = e.UserPhotoe,
                UserPhotoMimeType = e.UserPhotoMimeType,
                CreationTime = e.CreationTime,
                LastUpdateTime = e.LastUpdateTime,
                RoleId=e.RoleId,
                Role = e.Role
            };
            context.Set<User>().Add(user);
        }

        public void Delete(DalUser e)
        {
            var ormuser = context.Set<User>().FirstOrDefault(user => user.Id == e.Id);
            if (ormuser!=null)
            {
                context.Set<User>().Remove(ormuser);
            }
        }

        public void Update(DalUser entity)
        {
            throw new NotImplementedException();
        }
    }
}
