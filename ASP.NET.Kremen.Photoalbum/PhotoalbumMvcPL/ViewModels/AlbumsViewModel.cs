using System.Collections.Generic;
using BLL.Interfaces.Entities;

namespace PhotoalbumMvcPL.ViewModels
{
    public class AlbumsViewModel
    {
        public UserEntity UserFromSession { get; set; }
        public UserEntity UserFromAlbum { get; set; }

        public IEnumerable<HelperAlbumViewModel> AlbumList { get; set; } 
    }
}