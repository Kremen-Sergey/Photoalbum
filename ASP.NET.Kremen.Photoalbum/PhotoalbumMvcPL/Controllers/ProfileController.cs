using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using PhotoalbumMvcPL.Infrastructure;
using PhotoalbumMvcPL.ViewModels;

namespace PhotoalbumMvcPL.Controllers
{
    public class ProfileController : Controller
    {

        #region fields
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly IRoleService roleService;
        #endregion

        #region constructor
        public ProfileController(IAlbumService albumService, IUserService userService, IRoleService roleService)
        {
            this.albumService = albumService;
            this.userService = userService;
            this.roleService = roleService;
        }
        #endregion

        #region albums
        [HttpGet]
        public ActionResult Albums(SessionWrapper sessionWrapper, int userId)
        {

            UserEntity userFromAlbum = userService.GetAll().FirstOrDefault(u => u.Id == userId);
            if (userFromAlbum == null) { return RedirectToAction("Me", "Profile"); }
            UserEntity userFromSession = null;
            if (sessionWrapper.Email != null)
            {
                userFromSession =userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == sessionWrapper.Email.ToUpper());
                if (userFromSession == null) { return RedirectToAction("Login", "Account");}    
            }
            AlbumsViewModel model=new AlbumsViewModel();
            model.UserFromSession = userFromSession;
            model.UserFromAlbum = userFromAlbum;
            model.AlbumList = albumService.GetAll()
                .Where(album => album.UserId == userId)
                .Select(album => new HelperAlbumViewModel()
                {
                    Id = album.Id,
                    AlbumName = album.AlbumName,
                    Description = album.Description,
                    UserId = album.UserId,
                    UserName = userService.GetById(album.UserId).UserName,
                    CreationTime = album.CreationTime
                });

            return View(model);
        }
        #endregion

        #region new
        [HttpGet]
        public ActionResult New(SessionWrapper sessionWrapper)
        {
            if (sessionWrapper.Email != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult New(SessionWrapper sessionWrapper, HelperAlbumViewModel viewModel)
        {
            if (sessionWrapper.Email!=null)
            {
            var email = sessionWrapper.Email;
            var userFromSession = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            if (userFromSession == null) { return RedirectToAction("Login", "Account"); }
                if (ModelState.IsValid)
                {           
                    albumService.Create(new AlbumEntity()
                    {
                        AlbumName=viewModel.AlbumName,
                        Description=viewModel.Description,
                        UserId =userFromSession.Id,
                        CreationTime = DateTime.Now
                     });                  
                }
                return RedirectToAction("Albums", "Profile", new { userId = viewModel.UserId });
            }
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region delete
        public ActionResult Delete(SessionWrapper sessionWrapper, int albumId)
        {
            AlbumEntity albumEntity = albumService.GetAll().FirstOrDefault(a => a.Id == albumId);
            if (albumEntity == null) { return RedirectToAction("Me", "Profile"); }
            if (sessionWrapper.Email!= null)
            {
                var email = sessionWrapper.Email;
                var userFromSession = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
                if (userFromSession == null) { return RedirectToAction("Login", "Account"); }
                var album = albumService.GetAll().FirstOrDefault(u => u.Id == albumId);
                if (album != null)
                {
                    if (userFromSession.Id == album.UserId)
                    {
                        albumService.Delete(album);
                    }
                }
                return RedirectToAction("Albums", "Profile", new { userId = userFromSession.Id });
            }
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region me
        public ActionResult Me(SessionWrapper sessionWrapper)
        {
            if (sessionWrapper.Email!=null)
            {
                var email = sessionWrapper.Email;           
                var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
                if (user != null)
                {
                    return RedirectToAction("Albums", "Profile", new { userId = user.Id });
                }                               
            }
                return RedirectToAction("Login", "Account");
        }
        #endregion

    }
}
