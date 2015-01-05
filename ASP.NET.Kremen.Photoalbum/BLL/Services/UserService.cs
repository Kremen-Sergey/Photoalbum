using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork uow;
        private readonly IUserRepository userRepository;

        public UserService(IUnitOfWork uow, IUserRepository repository)
        {
            this.uow = uow;
            this.userRepository = repository;
            Debug.WriteLine("User service create!");
        }

        public IEnumerable<UserEntity> GetAll()
        {
                return userRepository.GetAll().Select(user => user.ToBllUser());
        }

        public void Create(UserEntity user)
        {
            userRepository.Create(user.ToDalUser());
            uow.Commit();
        }

        public void Delete(UserEntity user)
        {
            userRepository.Delete(user.ToDalUser());
            uow.Commit();
        }

        public UserEntity GetById(int id)
        {
            if (userRepository.GetById(id) == null) {return null;}
            return userRepository.GetById(id).ToBllUser();
        }
    }
}

