using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoalbumMvcPL.ViewModels
{
    public class CommentViewModel
    {
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [ScaffoldColumn(false)]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte[] UserPhotoe { get; set; }
        public string UserPhotoMimeType { get; set; }
        public int PhotoId { get; set; }
        public string TextComment { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}