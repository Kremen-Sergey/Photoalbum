using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace PhotoalbumMvcPL.ViewModels
{
    public class PhotoViewModel
    {
        public PhotoeEntity Photo { get; set; }
        public UserEntity UserFromSession { get; set; }
        public UserEntity UserFromAlbum { get; set; }
        public AlbumEntity Album { get; set; }
        public IEnumerable<CommentViewModel> CommentList { get; set; }
        public IEnumerable<HelperPhotoViewModel> PhotoList { get; set; } 
    }
}