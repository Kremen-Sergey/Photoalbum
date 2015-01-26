using System;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using PhotoalbumMvcPL.Infrastructure;
using PhotoalbumMvcPL.Providers;
using PhotoalbumMvcPL.ViewModels;
using BLL.Interfaces.Services;
using BLL.Mappers;


namespace PhotoalbumMvcPL.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        #region fields
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        #endregion

        #region constructor
        public AccountController(IUserService userService,IRoleService roleService)
        {
            this.userService = userService;
            this.roleService = roleService;
        }
        #endregion

        #region all users
        public ActionResult AllUsers()
        {

            return View(userService.GetAll()// IEnumerable<UserEntity> GetAll();
                .Select(user => new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    CreationDate = user.CreationTime,
                    Role=roleService.GetById(user.RoleId).RoleName,
                    UserPhotoe = user.UserPhotoe,
                    UserPhotoMimeType= user.UserPhotoMimeType,
                    LastUpdateTime= user.LastUpdateTime
                }));
        }
        #endregion

        #region login
       [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(SessionWrapper sessionWrapper, LogOnViewModel viewModel, string returnUrl)
        {
          
            if (ModelState.IsValid)
            {
                if (((CustomMembershipProvider)Membership.Provider).ValidateUser(viewModel.Email, viewModel.Password))
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, viewModel.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        sessionWrapper.Email = viewModel.Email;
                            
                        return RedirectToAction("Me", "Profile");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            
            return View(viewModel);
        }

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        #endregion

        # region register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(SessionWrapper sessionWrapper, RegisterViewModel viewModel, HttpPostedFileBase image)
        {
            if (viewModel.Captcha != (string)Session[CaptchaImage.CaptchaValueKey])/////////////////////
            {
                ModelState.AddModelError("Captcha", "Текст с картинки введен неверно");
                return View(viewModel);
            }
            var anyUser = userService.GetAll().Any(u => u.Email.Contains(viewModel.Email));
            if (anyUser)
            {
                ModelState.AddModelError("Email", "Пользователь с таким адресом уже зарегистрирован");
                return View(viewModel);
            }
            if (ModelState.IsValid)
            {
                try
                {

                    if (image != null)
                    {
                        if (!((image.ContentType == "image/jpeg") || (image.ContentType == "image/pjpeg") || (image.ContentType == "image/gif") || (image.ContentType == "image/png") || (image.ContentType == "image/svg+xml") || (image.ContentType == "image/tiff") || (image.ContentType == "image/vnd.microsoft.icon") || (image.ContentType == "image/vnd.wap.wbmp"))) { throw new Exception(); }
                        viewModel.UserPhotoMimeType = image.ContentType;
                        viewModel.UserPhotoe = new byte[image.ContentLength];
                        image.InputStream.Read(viewModel.UserPhotoe, 0, image.ContentLength);
                    }
                    else
                    {
                        string filePath = Server.MapPath("~/image/defaultAvatar.jpg"); //relative path for constructor
                        byte[] defaultAvatar = System.IO.File.ReadAllBytes(filePath); //read image from path
                        viewModel.UserPhotoMimeType = "image/jpeg";
                        viewModel.UserPhotoe = defaultAvatar;
                    }
                }
                catch (Exception e)
                {
                    ViewBag.Error = "Возможная причина ошибки: поддерживаются только файлы с расширением jpg, jpeg, png, tiff, gif, svg ";
                    return View("Error", (object) Request.UrlReferrer.Segments[2]);
                }
                sessionWrapper.Email = viewModel.Email;
                MembershipUser membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(viewModel.UserName, viewModel.Password, viewModel.Email, viewModel.UserPhotoMimeType, viewModel.UserPhotoe);           
                if (membershipUser != null)
                {
                    FormsAuthentication.SetAuthCookie(viewModel.Email, false);
                    return RedirectToAction("Me", "Profile");
                }
                else
                {
                    ModelState.AddModelError("", "Ошибка при регистрации");
                }
            }
            return View(viewModel);
        }
        # endregion

        #region captcha
        public ActionResult Captcha()
        {
            Session[CaptchaImage.CaptchaValueKey] =
                new Random(DateTime.Now.Millisecond).Next(1111, 9999).ToString(CultureInfo.InvariantCulture);
            var ci = new CaptchaImage(Session[CaptchaImage.CaptchaValueKey].ToString(), 211, 50, "Helvetica");
            this.Response.Clear();
            this.Response.ContentType = "image/jpeg";
            ci.Image.Save(this.Response.OutputStream, ImageFormat.Jpeg);
            ci.Dispose();
            return null;
        }
        #endregion 

        #region menu
        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView();
        }
        #endregion

        # region get image
        public FileContentResult GetImage(int userId)
        {
            var user = userService.GetAll().Select(u => u.ToDalUser()).FirstOrDefault(u => u.Id == userId);//DalUser
            if (user != null)
            {
                return File(user.UserPhotoe, user.UserPhotoMimeType);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region delete user
        public ActionResult Delete(int userId)
        {
            var user = userService.GetAll().FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                userService.Delete(user);
            }
            return RedirectToAction("AllUsers", "Account");
        }
        #endregion

    }
}
