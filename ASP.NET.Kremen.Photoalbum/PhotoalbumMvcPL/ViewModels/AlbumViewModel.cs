using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace PhotoalbumMvcPL.ViewModels
{
    public class AlbumViewModel
    {
        public UserEntity UserFromSession { get; set; }
        public UserEntity UserFromAlbum { get; set; }
        public AlbumEntity Album { get; set; }

        public IEnumerable<HelperPhotoViewModel> PhotoList { get; set; }

        public PagingInfo PagingInfo { get; set; }


    }
}