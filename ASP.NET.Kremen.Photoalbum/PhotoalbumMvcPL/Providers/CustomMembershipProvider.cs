using System;
using System.Linq;
using System.Web.Helpers;
using System.Web.Security;
using ORM;

namespace PhotoalbumMvcPL.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {

        public MembershipUser CreateUser(string userName, string password, string email, string userPhotoMimeType, byte[] userPhotoe)
        {
            MembershipUser membershipUser = GetUser(email, false);
            if (membershipUser != null)
            {
                return null;
            }
            using (var context = new EntityModel())
            {
                var user = new User
                {
                    UserName = userName,
                    Email = email,
                    Password = Crypto.HashPassword(password),
                    CreationTime = DateTime.Now,
                    LastUpdateTime = DateTime.Now,
                    UserPhotoMimeType = userPhotoMimeType, 
                    UserPhotoe = userPhotoe
                };
                var role = context.Roles.FirstOrDefault(r => r.RoleName == "user");
                if (role != null)
                {
                    user.RoleId = role.Id;
                }
                context.Users.Add(user);
                context.SaveChanges();
                membershipUser = GetUser(email, false);
                return membershipUser;
            }

        }

        public override bool ValidateUser(string email, string password)
        {
            using (var context = new EntityModel())
            {
                User user = (from u in context.Users
                             where u.Email.ToUpper() == email.ToUpper()
                             select u).FirstOrDefault();
                if (user != null && Crypto.VerifyHashedPassword(user.Password, password))
                {
                    return true;
                }
            }
            return false;
        }

        public override MembershipUser GetUser(string email, bool userIsOnline)
        {
            using (var context = new EntityModel())
            {
                var user = (from u in context.Users
                            where u.Email.ToUpper() == email.ToUpper()
                            select u).FirstOrDefault();
                if (user == null) return null;
                var memberUser = new MembershipUser("CustomMembershipProvider", user.Email,
                    null, null, null, null,
                    false, false, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue, DateTime.MinValue);
                return memberUser;
            }
        }

        public override MembershipUser CreateUser(string username,  string password, string email, string passwordQuestion,
            string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
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