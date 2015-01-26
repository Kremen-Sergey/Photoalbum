using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL.Interfaces.Entities;
using BLL.Interfaces.Services;
using BLL.Mappers;
using Microsoft.Ajax.Utilities;
using PhotoalbumMvcPL.Infrastructure;
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
        public ActionResult Album(SessionWrapper sessionWrapper, int albumId)
        {
            var album = albumService.GetAll().FirstOrDefault(a => a.Id == albumId);
            UserEntity userFromSession = null;
            if (album == null) { return RedirectToAction("Me", "Profile"); }
            if (sessionWrapper.Email != null)
            {
                userFromSession = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == sessionWrapper.Email.ToUpper());
                if (userFromSession == null) {return RedirectToAction("Login", "Account"); }
            }
            var userFromAlbum= userService.GetById(albumService.GetById(albumId).UserId);
            var model = new AlbumViewModel();
            model.UserFromSession = userFromSession;
            model.UserFromAlbum = userFromAlbum;
            model.Album = album;
            model.PhotoList=photoService.GetAll().
                    Where(photo => photo.AlbumId == albumId)
                    .Select(photo => new HelperPhotoViewModel()
                    {  
                        Id =photo.Id,
                        AlbumId=photo.AlbumId,
                        Like=photo.Like,
                        Description=photo.Description,
                        ImagePhotoe=photo.ImagePhotoe,
                        ImagePhotoMimeType=photo.ImagePhotoMimeType,
                        AddTime=photo.AddTime
                    });
            return
                View(model);
        }

        [HttpPost]
        public ActionResult Album(string albumId, HttpPostedFileBase image)
        {

            if ((image != null) && (albumId != null))
            {
                int albumid = int.Parse(albumId);
                try
                {
                    if (!((image.ContentType == "image/jpeg") || (image.ContentType == "image/pjpeg") || (image.ContentType == "image/gif") || (image.ContentType == "image/png") || (image.ContentType == "image/svg+xml") || (image.ContentType == "image/tiff") || (image.ContentType == "image/vnd.microsoft.icon") || (image.ContentType == "image/vnd.wap.wbmp"))) { throw new Exception(); }
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
                    ViewBag.Error="Возможная причина ошибки: поддерживаются только файлы с расширением jpg, jpeg, png, tiff, gif, svg ";
                    if (Request.UrlReferrer != null)
                    {
                        return View("Error", (object) Request.UrlReferrer.Segments[2]);
                    }
                    else
                    {
                        return RedirectToAction("Me", "Profile");
                    }
                }
                return RedirectToAction("Album", "Photo", new { albumId = albumid });              
            }
            return RedirectToAction("Album", "Photo", new { albumId = int.Parse(albumId) });  
        }
        #endregion

        #region picture
        [HttpGet]
        public ActionResult Picture(SessionWrapper sessionWrapper, int photoId)
        {
            PhotoeEntity photo = photoService.GetAll().FirstOrDefault(u => u.Id == photoId);
            if (photo == null) { return RedirectToAction("Me", "Profile"); }
            UserEntity userFromSession = null;
            if (sessionWrapper.Email != null)
            {
                userFromSession = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == sessionWrapper.Email.ToUpper());
                if (userFromSession == null) { return RedirectToAction("Login", "Account"); }
            }
            var album = albumService.GetById(photo.AlbumId);
            var userFromAlbum = userService.GetById(album.UserId);
            var commentList = GetComments(photoId);
            var model = new PhotoViewModel();
            model.Album = album;
            model.CommentList = commentList;
            model.Photo = photo;
            model.UserFromAlbum = userFromAlbum;
            model.UserFromSession = userFromSession;
            model.PhotoList=photoService.GetAll().
                            Where(p => p.AlbumId == album.Id) // IEnumerable<AlbumEntity> GetAll();
                            .Select(p => new HelperPhotoViewModel()
                            {
                                Id = p.Id,
                                AlbumId = p.AlbumId,
                                Like = p.Like,
                                Description = p.Description,
                                ImagePhotoe = p.ImagePhotoe,
                                ImagePhotoMimeType = p.ImagePhotoMimeType,
                                AddTime = p.AddTime
                            });
            return View(model);
        }

        [HttpPost]
        public ActionResult Picture(SessionWrapper sessionWrapper, string comment, string photoId)
        {
            int photoID = int.Parse(photoId);
            if (!comment.IsNullOrWhiteSpace())
            {
                var userFromSession = userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() ==sessionWrapper.Email.ToUpper());
                if (userFromSession == null) { return RedirectToAction("Login", "Account"); }
                if (photoId == null) { return RedirectToAction("Me", "Profile"); }
                var commentEntity = new CommentEntity()
                {
                    UserId = userFromSession.Id,
                    PhotoId = photoID,
                    TextComment = comment,
                    CreateTime = DateTime.Now
                };
                commentService.Create(commentEntity);
            }
            return RedirectToAction("Picture", "Photo", new { photoId = photoID });
        }
        #endregion

        #region get image
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
        public ActionResult Edit (SessionWrapper sessionWrapper, int photoId)
        {
            PhotoeEntity photo = photoService.GetAll().FirstOrDefault(u => u.Id == photoId);
            if (photo == null) { return RedirectToAction("Me", "Profile"); }
            if (sessionWrapper.Email != null)
            {
                var userFromSession =userService.GetAll().FirstOrDefault(u => u.Email.ToUpper() == sessionWrapper.Email.ToUpper());
                if (userFromSession == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var photoVM = new HelperPhotoViewModel()
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
        public ActionResult Edit(HelperPhotoViewModel photoVM)
        {
            if (ModelState.IsValid)
            {
                PhotoeEntity photoEntity = photoService.GetAll().FirstOrDefault(u => u.Id == photoVM.Id);
                if (photoEntity != null)
                {
                    photoEntity.Description = photoVM.Description;
                }
                photoService.Update(photoEntity);
            }
            return RedirectToAction("Picture", "Photo", new { photoId = photoVM.Id }); 
        }
        #endregion

        #region view picture
        public ActionResult ViewPicture(int photoId)
        {
            PhotoeEntity photo = photoService.GetAll().FirstOrDefault(u => u.Id == photoId);
            if (photo == null) { return RedirectToAction("Me", "Profile"); }
            return View(photoId);
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
            var albumid = photo.AlbumId;
            photoService.Delete(photo);
            return RedirectToAction("Album", "Photo" , new {albumId=albumid});
        }
        #endregion

        #region search
        [HttpGet]
        public ActionResult Search()
        {
            return PartialView("SearchField");
        }

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

        #region slider
        [HttpGet]
        public ActionResult Slider(SessionWrapper sessionWrapper, int albumId)
        {
            AlbumEntity album = albumService.GetAll().FirstOrDefault(a => a.Id == albumId);
            if (album == null) { return RedirectToAction("Me", "Profile"); }
            ViewBag.AlbumId = albumId;
            ViewBag.AlbumName = albumService.GetById(albumId).AlbumName;
            ViewBag.UserNameFromAlbum = userService.GetById(albumService.GetById(albumId).UserId).UserName;
            ViewBag.UserIdFromAlbum = userService.GetById(albumService.GetById(albumId).UserId).Id;
            return
                View(photoService.GetAll().
                    Where(photo => photo.AlbumId == albumId) // IEnumerable<AlbumEntity> GetAll();
                    .Select(photo => new HelperPhotoViewModel()
                    {
                        Id = photo.Id,
                        AlbumId = photo.AlbumId,
                        Like = photo.Like,
                        Description = photo.Description,
                        ImagePhotoe = photo.ImagePhotoe,
                        ImagePhotoMimeType = photo.ImagePhotoMimeType,
                        AddTime = photo.AddTime
                    }));
        }
        #endregion

    }
}
