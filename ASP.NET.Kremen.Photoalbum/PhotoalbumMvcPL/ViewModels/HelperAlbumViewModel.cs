using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoalbumMvcPL.ViewModels
{
    public class HelperAlbumViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Display(Name = "Название альбома")]
        [Required(ErrorMessage = "Поле не должно быть пустым")]
        [StringLength(50, ErrorMessage = "Название альбома должно содержать не более 50 символов")]
        public string AlbumName { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Имя пользователя")]
        public string UserName { get; set; }
        [ScaffoldColumn(false)]
        public int UserId { get; set; }
        [Display(Name = "Дата создания")]
        public DateTime? CreationTime { get; set; }
    }
}