using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Security;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;

namespace PhotoalbumMvcPL.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {

        public MembershipUser CreateUser(string userName, string password, string email, string userPhotoMimeType, byte[] userPhotoe, IUserService userService, IRoleService roleService)
        {
            MembershipUser membershipUser = GetUser(email, false, userService, roleService);
            if (membershipUser != null)
            {
                return null;
            }
            var user = new UserEntity()
            {
                UserName = userName,
                Email = email,
                Password = Crypto.HashPassword(password),
                CreationTime = DateTime.Now,
                LastUpdateTime = DateTime.Now,
                UserPhotoMimeType = userPhotoMimeType,
                UserPhotoe = userPhotoe
            };
            var role = roleService.GetAll().FirstOrDefault(r => r.RoleName.ToUpper() == "USER");
                if (role != null)
                {
                    user.RoleId = role.Id;
                }
            userService.Create(user);
            membershipUser = GetUser(email, false, userService, roleService);
            return membershipUser;
        }

        public bool ValidateUser(string email, string password, IUserService userService, IRoleService roleService)

        {
            var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
                {
                    return true;
                }
            return false;
        }

        public MembershipUser GetUser(string email, bool userIsOnline, IUserService userService,IRoleService roleService)
        {
            var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            if (user == null) return null;
            var memberUser = new MembershipUser("CustomMembershipProvider", user.Email,
                null, null, null, null,
                false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
            return memberUser;
        }
      
        public override MembershipUser CreateUser(string username,  string password, string email, string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string email, string password)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override string ApplicationName { get; set; }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
    }
}