using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using Microsoft.Ajax.Utilities;
using PhotoalbumMvcPL.ViewModels;

namespace PhotoalbumMvcPL.Controllers
{
    public class PhotoController : Controller
    {

        #region fields

        private readonly IPhotoeService photoService;
        private readonly IAlbumService albumService;
        private readonly IUserService userService;
        private readonly ICommentService commentService;
        #endregion

        #region constructor
        public PhotoController(IPhotoeService photoService, IAlbumService albumService, IUserService userService, ICommentService commentService)
        {
            this.photoService = photoService;
            this.albumService = albumService;
            this.userService = userService;
            this.commentService = commentService;
        }
        #endregion

        #region Album

        [HttpGet]
        public ActionResult Album(int albumId)
        {
            AlbumEntity album = albumService.GetAll().FirstOrDefault(a => a.Id == albumId);
            if (album == null) { return RedirectToAction("Me", "Profile"); }
            Session["UserIdFromSession"] = null;
            if (Session["Email"] != null)
            {
                var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == Session["Email"].ToString().ToUpper());
                if (user == null) {return RedirectToAction("Login", "Account"); }
                Session["UserIdFromSession"] = user.Id;
            }
            Session["AlbumId"] = albumId;
            Session["AlbumName"] = albumService.GetById(albumId).AlbumName;
            Session["UserNameFromAlbum"] = userService.GetById(albumService.GetById(albumId).UserId).UserName;
            Session["UserIdFromAlbum"] = userService.GetById(albumService.GetById(albumId).UserId).Id;
            return
                View(photoService.GetAll().
                    Where(photo => photo.AlbumId == albumId) // IEnumerable<AlbumEntity> GetAll();
                    .Select(photo => new PhotoViewModel()
                    {  
                        Id =photo.Id,
                        AlbumId=photo.AlbumId,
                        Like=photo.Like,
                        Description=photo.Description,
                        ImagePhotoe=photo.ImagePhotoe,
                        ImagePhotoMimeType=photo.ImagePhotoMimeType,
                        AddTime=photo.AddTime
                    }));
        }

        [HttpPost]
        public ActionResult Album(IEnumerable<PhotoViewModel> viewModel, HttpPostedFileBase image)
        {
            
            if ((image != null)&&(Session["AlbumId"]!=null))
            {
                int albumid = (int)Session["AlbumId"];
                try
                {       
                    byte[] photo = new byte[image.ContentLength];
                    image.InputStream.Read(photo, 0, image.ContentLength);
                    photoService.Create(new PhotoeEntity()
                    {
                        AlbumId = albumid,
                        Like = 0,
                        Description = "...",
                        ImagePhotoe = photo,
                        ImagePhotoMimeType = image.ContentType,
                        AddTime = DateTime.Now

                    });
                }
                catch (Exception e)
                {
                    ViewBag.Error="Возможная причина ошибки: загружаемый файл не яваляется изображением или превышает размер 100 Мб";
                    return View("Error", (object)Request.UrlReferrer.Segments[2]); 
                }
                return RedirectToAction("Album", "Photo", new { albumId = albumid });              
            }
            return RedirectToAction("Album", "Photo", new { albumId = Session["AlbumId"] });  
        }
        #endregion

        #region picture
        [HttpGet]
        public ActionResult Picture(int photoId)
        {
            PhotoeEntity photo = photoService.GetAll().FirstOrDefault(u => u.Id == photoId);
            if (photo == null) { return RedirectToAction("Me", "Profile"); }
            Session["UserIdFromSession"] = null;
            if (Session["Email"] != null)
            {
                var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == Session["Email"].ToString().ToUpper());
                if (user == null) { return RedirectToAction("Login", "Account"); }
                Session["UserIdFromSession"] = user.Id;
            }
            Session["PhotoId"] = photoId;
            Session["Description"] = photoService.GetById(photoId).Description;
            Session["AlbumId"] = photoService.GetById(photoId).AlbumId;
            Session["AlbumName"] = albumService.GetById((int)Session["AlbumId"]).AlbumName;
            Session["UserIdFromAlbum"] = albumService.GetById((int)Session["AlbumId"]).UserId;
            Session["UserNameFromAlbum"] = userService.GetById((int)Session["UserIdFromAlbum"]).UserName;
            ViewBag.CommentList = GetComments(photoId);
            return View(photoService.GetAll().
                            Where(p => p.AlbumId == (int)Session["AlbumId"]) // IEnumerable<AlbumEntity> GetAll();
                            .Select(p => new PhotoViewModel()
                            {
                                Id = p.Id,
                                AlbumId = p.AlbumId,
                                Like = p.Like,
                                Description = p.Description,
                                ImagePhotoe = p.ImagePhotoe,
                                ImagePhotoMimeType = p.ImagePhotoMimeType,
                                AddTime = p.AddTime
                            }));
        }

        [HttpPost]
        public ActionResult Picture(string comment)
        {
            if (!comment.IsNullOrWhiteSpace())
            {
                Session["UserIdFromSession"] = null;
                var user = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == Session["Email"].ToString().ToUpper());
                if (user == null) { return RedirectToAction("Login", "Account"); }
                Session["UserIdFromSession"] = user.Id;
                if (Session["PhotoId"] == null) { return RedirectToAction("Me", "Profile"); }
                CommentEntity commentEntity = new CommentEntity()
                {
                    UserId = user.Id,
                    PhotoId = (int)Session["PhotoId"],
                    TextComment = comment,
                    CreateTime = DateTime.Now
                };
                commentService.Create(commentEntity);
            }
            return RedirectToAction("Picture", "Photo", new { photoId = (int)Session["PhotoId"] }); ;
        }
        public FileContentResult GetImage(int photoId)
        {
            var photo = photoService.GetAll().Select(p => p.ToDalPhotoe()).FirstOrDefault(p => p.Id == photoId);//DalPhoto
            if (photo != null)
            {
                return File(photo.ImagePhotoe, photo.ImagePhotoMimeType);
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region edit
        [HttpGet]
        public ActionResult Edit(int photoId)
        {
            PhotoeEntity photo = photoService.GetAll().FirstOrDefault(u => u.Id == photoId);
            if (photo == null) { return RedirectToAction("Me", "Profile"); }
            Session["UserIdFromSession"] = null;
            if (Session["Email"] != null)
            {
                var user =
                    userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == Session["Email"].ToString().ToUpper());
                if (user == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                Session["UserIdFromSession"] = user.Id; 
                PhotoViewModel photoVM = new PhotoViewModel()
                {
                    Id = photo.Id,
                    AlbumId = photo.AlbumId,
                    Like = photo.Like,
                    Description = photo.Description,
                    ImagePhotoe = photo.ImagePhotoe,
                    ImagePhotoMimeType = photo.ImagePhotoMimeType,
                    AddTime = photo.AddTime
                };
                return View(photoVM);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(PhotoViewModel photoVM)
        {
            if (ModelState.IsValid)
            {
                PhotoeEntity photoEntity = photoService.GetAll().FirstOrDefault(u => u.Id == (int) Session["PhotoId"]);
                if (photoEntity != null)
                {
                    photoEntity.Description = photoVM.Description;
                }
                photoService.Update(photoEntity);
            }
            return RedirectToAction("Picture", "Photo", new { photoId = (int)Session["PhotoId"] }); 
        }
        #endregion

        #region view picture
        public ActionResult ViewPicture(int photoId)
        {
            PhotoeEntity photo = photoService.GetAll().FirstOrDefault(u => u.Id == photoId);
            if (photo == null) { return RedirectToAction("Me", "Profile"); }
            return View();
        }
        #endregion

        #region get comments
        public IEnumerable<CommentViewModel> GetComments(int photoId)
        {
            var photo = photoService.GetAll().Select(p => p.ToDalPhotoe()).FirstOrDefault(p => p.Id == photoId);//DalPhoto
            if (photo != null)
            {
                IEnumerable<CommentEntity> commentList = commentService.GetAll().Where(u => (u.PhotoId == photoId));
                List<CommentViewModel> commentViewModelList=new List<CommentViewModel>();
                foreach (CommentEntity comment in commentList)
                {

                    UserEntity user = userService.GetById(comment.UserId);
                    string userName = "DELETED";
                    byte[] userPhoto = null;
                    string userPhotoMimeType = null;
                    if (user != null)
                    {
                        userName = user.UserName;
                        userPhoto = user.UserPhotoe;
                        userPhotoMimeType = user.UserPhotoMimeType;
                    }
                    commentViewModelList.Add(new CommentViewModel()
                    {
                        Id = comment.Id,
                        PhotoId = comment.PhotoId,
                        UserId = comment.UserId,
                        UserName = userName,
                        UserPhotoe = userPhoto,
                        UserPhotoMimeType = userPhotoMimeType,
                        TextComment = comment.TextComment,
                        CreateTime = comment.CreateTime
                    });
                    
                }
                return commentViewModelList;
            }
            else
            {
            return null;
            }
        }
        #endregion

        #region delete
        public ActionResult Delete(int photoId)
        {
            PhotoeEntity photo = photoService.GetAll().FirstOrDefault(u => u.Id == photoId);
            if (photo == null) { return RedirectToAction("Me", "Profile"); }
            Session["PhotoId"] = null;
            photoService.Delete(photo);
            return RedirectToAction("Album", "Photo" , new {albumId=(int)Session["AlbumId"]});
        }
        #endregion

        #region search
        //[ChildActionOnly]
        [HttpGet]
        public ActionResult Search()
        {
            return PartialView("SearchField");
        }

        //[ChildActionOnly]
        [HttpPost]
        public ActionResult Search(string strRequest)
        {
            return RedirectToAction("SearchResult", "Photo", new { stringRequest = strRequest });
        }

        public ActionResult SearchResult(string stringRequest)
        {
            if (stringRequest == null)
            {
                return null;
            }
            IEnumerable<PhotoeEntity> photoes = photoService.GetAll();
            if (photoes == null)
            {
                return null;
            }
            IEnumerable<PhotoeEntity> selectedPhotoes = from p in photoes
                                                        where p.Description.ToUpper().Contains(stringRequest.ToUpper())
                                                        select p;
            return View(selectedPhotoes.Select(p => new SearchPhotoViewModel()
            {
                Id = p.Id,
                Description = p.Description,
                ImagePhotoe = p.ImagePhotoe,
                ImagePhotoMimeType = p.ImagePhotoMimeType,
            }));
        }

        #endregion

    }
}
