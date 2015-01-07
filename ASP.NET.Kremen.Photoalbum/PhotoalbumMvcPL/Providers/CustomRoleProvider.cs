using System;
using System.Linq;
using System.Web.Security;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;

namespace PhotoalbumMvcPL.Providers
{
    public class CustomRoleProvider : RoleProvider
    {

        public bool IsUserInRole(string email, string roleName, IUserService userService, IRoleService roleService)
        {
            var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            if (user == null) return false;
            var role = roleService.GetAll().FirstOrDefault(r => r.Id == user.RoleId);
            if (role != null && role.RoleName.ToUpper() == roleName.ToUpper()) {return true;}
            return false;
            //using (var context = new EntityModel())
            //{
            //    User user = (from u in context.Users
            //                 where u.Email == email
            //                 select u).FirstOrDefault();
            //    if (user == null) return false;
            //    Role userRole = context.Roles.Find(user.RoleId);
            //    if (userRole != null && userRole.RoleName == roleName)
            //    {
            //        return true;
            //    }
            //    return false;
        }

        public string[] GetRolesForUser(string email, IUserService userService, IRoleService roleService)
        {
            var roles = new string[] { };
            var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            if (user == null) return roles;
            var role = roleService.GetAll().FirstOrDefault(r => r.Id == user.RoleId);
            if (role != null)
            {
                roles = new string[] { role.RoleName };
            }
            return roles;
            //using (var context = new EntityModel())
            //{
            //    var roles = new string[] { };
            //    var user = context.Users.FirstOrDefault(u => u.Email == email);
            //    if (user == null) return roles;
            //    var userRole = user.Role;
            //    if (userRole != null)
            //    {
            //        roles = new string[] { userRole.RoleName };
            //    }
            //    return roles;
            //}
        }

        public void CreateRole(string roleName, IUserService userService, IRoleService roleService)
        {
            var role = new RoleEntity() {RoleName = roleName};
            roleService.Create(role);
            //var newRole = new Role() { RoleName = roleName };
            //using (var context = new EntityModel())
            //{
            //    context.Roles.Add(newRole);
            //    context.SaveChanges();
            //}
        }

        public override bool IsUserInRole(string email, string roleName)
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string email)
        {
            throw new NotImplementedException();
            //return GetRolesForUser(email, userService, roleService);
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
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