using BLL.Interfaces.Entities;
using DAL.Interface.DTO;

namespace BLL.Mappers
{
    public static class BllEntityMapper
    {

        #region Mapper for User

        public static DalUser ToDalUser(this UserEntity userEntity)
        {
            return new DalUser()
            {
                Id = userEntity.Id,
                UserName = userEntity.UserName,
                Email = userEntity.Email,
                Password=userEntity.Password,
                RoleId = userEntity.RoleId,
                UserPhotoe = userEntity.UserPhotoe,
                UserPhotoMimeType = userEntity.UserPhotoMimeType,
                CreationTime = userEntity.CreationTime,
                LastUpdateTime = userEntity.LastUpdateTime
                };
        }
       public static UserEntity ToBllUser(this DalUser dalUser)
        {
            return new UserEntity()
            {
                Id = dalUser.Id,
                UserName = dalUser.UserName,
                Email = dalUser.Email,
                Password = dalUser.Password,
                RoleId = dalUser.RoleId,
                UserPhotoe = dalUser.UserPhotoe,
                UserPhotoMimeType = dalUser.UserPhotoMimeType,
                CreationTime = dalUser.CreationTime,
                LastUpdateTime = dalUser.LastUpdateTime
               
            };
        }

        #endregion

        #region Mapper for Role

        public static DalRole ToDalRole(this RoleEntity roleEntity)
        {
            return new DalRole()
            {
                Id = roleEntity.Id,
                RoleName = roleEntity.RoleName
            };
        }

        public static RoleEntity ToBllRole(this DalRole dalRole)
        {
            return new RoleEntity()
            {
                Id = dalRole.Id,
                RoleName = dalRole.RoleName,
            };
        }

        #endregion

        #region Mapper for Photoe

        public static DalPhotoe ToDalPhotoe(this PhotoeEntity photoerEntity)
        {
            return new DalPhotoe()
            {
                Id = photoerEntity.Id,
                AlbumId = photoerEntity.AlbumId,
                Like = photoerEntity.Like,
                Description = photoerEntity.Description,
                ImagePhotoe = photoerEntity.ImagePhotoe,
                ImagePhotoMimeType = photoerEntity.ImagePhotoMimeType,
                AddTime = photoerEntity.AddTime
            };
        }

        public static PhotoeEntity ToBllPhotoe(this DalPhotoe dalPhotoe)
        {
            return new PhotoeEntity()
            {
                Id = dalPhotoe.Id,
                AlbumId = dalPhotoe.AlbumId,
                Like = dalPhotoe.Like,
                Description = dalPhotoe.Description,
                ImagePhotoe = dalPhotoe.ImagePhotoe,
                ImagePhotoMimeType = dalPhotoe.ImagePhotoMimeType,
                AddTime = dalPhotoe.AddTime
            };
        }

        #endregion

        #region Mapper for Comment

        public static DallComment ToDalComment(this CommentEntity commentEntity)
        {
            return new DallComment()
            {
                Id = commentEntity.Id,
                UserId = commentEntity.UserId,
                PhotoId = commentEntity.PhotoId,
                TextComment = commentEntity.TextComment,
                CreateTime = commentEntity.CreateTime
            };
        }

        public static CommentEntity ToBllComment(this DallComment dalComment)
        {
            return new CommentEntity()
            {
                Id = dalComment.Id,
                UserId = dalComment.UserId,
                PhotoId = dalComment.PhotoId,
                TextComment = dalComment.TextComment,
                CreateTime = dalComment.CreateTime
            };
        }

        #endregion

        #region Mapper for Album

        public static DalAlbum ToDalAlbum(this AlbumEntity albumEntity)
        {
            return new DalAlbum()
            {
                Id = albumEntity.Id,
                AlbumName = albumEntity.AlbumName,
                Description = albumEntity.Description,
                UserId = albumEntity.UserId,
                CreationTime = albumEntity.CreationTime
            };
        }

        public static AlbumEntity ToBllAlbum(this DalAlbum dalAlbum)
        {
            return new AlbumEntity()
            {
                Id = dalAlbum.Id,
                AlbumName = dalAlbum.AlbumName,
                Description = dalAlbum.Description,
                UserId = dalAlbum.UserId,
                CreationTime = dalAlbum.CreationTime
            };
        }

        #endregion

    }
}


     