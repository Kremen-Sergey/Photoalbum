using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoalbumMvcPL.ViewModels
{
    public class PhotoViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [ScaffoldColumn(false)]
        public int AlbumId { get; set; }
        public int Like { get; set; }
        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Описание должно содержать не более 50 символов")]
        public string Description { get; set; }
        public byte[] ImagePhotoe { get; set; }
        public string ImagePhotoMimeType { get; set; }
        [Display(Name = "Время добавления")]
        public DateTime? AddTime { get; set; }
    }
}