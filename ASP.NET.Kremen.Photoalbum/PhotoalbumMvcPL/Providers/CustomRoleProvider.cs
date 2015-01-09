using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;

namespace PhotoalbumMvcPL.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        
        public override bool IsUserInRole(string email, string roleName)
        {
            IUserService userService = DependencyResolver.Current.GetService<IUserService>();
            IRoleService roleService = DependencyResolver.Current.GetService<IRoleService>();
            var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            if (user == null) return false;
            var role = roleService.GetAll().FirstOrDefault(r => r.Id == user.RoleId);
            if (role != null && role.RoleName.ToUpper() == roleName.ToUpper()) {return true;}
            return false;
        }

        public override string[] GetRolesForUser(string email)
        {
            IUserService userService = DependencyResolver.Current.GetService<IUserService>();
            IRoleService roleService = DependencyResolver.Current.GetService<IRoleService>();
            var roles = new string[] { };
            var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            if (user == null) return roles;
            var role = roleService.GetAll().FirstOrDefault(r => r.Id == user.RoleId);
            if (role != null)
            {
                roles = new string[] { role.RoleName };
            }
            return roles;
        }

        public override void CreateRole(string roleName)
        {
            IRoleService roleService = DependencyResolver.Current.GetService<IRoleService>();
            var role = new RoleEntity() {RoleName = roleName};
            roleService.Create(role);
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string ApplicationName { get; set; }
    }
}