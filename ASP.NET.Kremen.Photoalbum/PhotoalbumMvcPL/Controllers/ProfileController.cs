using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
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
        public ActionResult Albums(int userId)
        {
            UserEntity userEntity = userService.GetAll().FirstOrDefault(u => u.Id == userId);
            if (userEntity == null) { return RedirectToAction("Me", "Profile"); }
            Session["UserIdFromSession"] = null;
            if (Session["Email"] != null)
            {
                var user =userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == Session["Email"].ToString().ToUpper());
                if (user == null) { return RedirectToAction("Login", "Account");}
                Session["UserIdFromSession"] = user.Id;    
            }
            Session["UserNameFromUserId"] = userService.GetById(userId).UserName;
            Session["UserIdFromAlbums"] = userId;
                return
                    View(albumService.GetAll()
                        .Where(album => album.UserId == userId) // IEnumerable<AlbumEntity> GetAll();
                        .Select(album => new AlbumViewModel()
                        {
                            Id = album.Id,
                            AlbumName = album.AlbumName,
                            Description = album.Description,
                            UserId = album.UserId,
                            UserName = userService.GetById(album.UserId).UserName,
                            CreationTime = album.CreationTime
                        }));
        }
        #endregion

        #region new
        [HttpGet]
        public ActionResult New()
        {
            if (Session["Email"] != null)
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult New(AlbumViewModel viewModel)
        {
            Session["UserIdFromSession"] = null;
            if (Session["Email"] != null)
            {
            var email = Session["Email"].ToString();
            var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
            if (user == null) { return RedirectToAction("Login", "Account"); }
            Session["UserIdFromSession"] = user.Id;    
                if (ModelState.IsValid)
                {           
                    albumService.Create(new AlbumEntity()
                    {
                        AlbumName=viewModel.AlbumName,
                        Description=viewModel.Description,
                        UserId = (int)Session["UserIdFromSession"],
                        CreationTime = DateTime.Now
                     });                  
                }
                return RedirectToAction("Albums", "Profile", new { userId = (int)Session["UserIdFromSession"] });
            }
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region delete
        public ActionResult Delete(int albumId)
        {
            AlbumEntity albumEntity = albumService.GetAll().FirstOrDefault(a => a.Id == albumId);
            if (albumEntity == null) { return RedirectToAction("Me", "Profile"); }
            Session["UserIdFromSession"] = null;
            if (Session["Email"] != null)
            {
                var email = Session["Email"].ToString();
                var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
                if (user == null) { return RedirectToAction("Login", "Account"); }
                Session["UserIdFromSession"] = user.Id;
                var album = albumService.GetAll().FirstOrDefault(u => u.Id == albumId);
                if (album != null)
                {
                    if ((int)Session["UserIdFromSession"] == album.UserId)
                    {
                        albumService.Delete(album);
                    }
                }
                return RedirectToAction("Albums", "Profile", new { userId = (int)Session["UserIdFromSession"] });
            }
            return RedirectToAction("Login", "Account");
        }
        #endregion

        #region me
        public ActionResult Me()
        {

            if (Session["Email"] != null)
            {
                Session["IsAdminFlag"] = null;
                var email = Session["Email"].ToString();           
                var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == email.ToUpper());
                if (user != null)
                {
                    var role = roleService.GetById(user.RoleId);
                    if (role.RoleName.ToUpper()=="ADMIN") {Session["IsAdminFlag"]=true;}
                    else {Session["IsAdminFlag"] = false;}
                    return RedirectToAction("Albums", "Profile", new { userId = user.Id });
                } 
                                
            }
                return RedirectToAction("Login", "Account");
        }
        #endregion

    }
}
