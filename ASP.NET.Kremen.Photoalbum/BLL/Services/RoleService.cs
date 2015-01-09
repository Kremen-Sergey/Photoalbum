using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using DAL.Interface.Repository;

namespace BLL.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork uow;
        private readonly IRoleRepository roleRepository;

        public RoleService(IUnitOfWork uow, IRoleRepository roleRepository)
        {
            this.uow = uow;
            this.roleRepository = roleRepository;
            Debug.WriteLine("Role service create!"); 
        }
        public IEnumerable<RoleEntity> GetAll()
        {
            {
                return roleRepository.GetAll().Select(role => role.ToBllRole());
            }
        }

        public void Create(RoleEntity role)
        {
            roleRepository.Create(role.ToDalRole());
            uow.Commit();
        }

        public void Delete(RoleEntity role)
        {
            roleRepository.Delete(role.ToDalRole());
        }

        public RoleEntity GetById(int id)
        {
            if (roleRepository.GetById(id) == null) { return null; }
            return roleRepository.GetById(id).ToBllRole();
        }
    }
}
