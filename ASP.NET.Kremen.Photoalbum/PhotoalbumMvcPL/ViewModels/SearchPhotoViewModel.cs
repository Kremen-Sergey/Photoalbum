using System.ComponentModel.DataAnnotations;

namespace PhotoalbumMvcPL.ViewModels
{
    public class SearchPhotoViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        public string Description { get; set; }
        public byte[] ImagePhotoe { get; set; }
        public string ImagePhotoMimeType { get; set; }
    }
}