using System;

namespace DAL.Interface.DTO
{
    public class DalPhotoe : IEntity
    {
        public int Id { get; set; }
        public int AlbumId { get; set; }
        public int Like { get; set; }
        public string Description { get; set; }
        public byte[] ImagePhotoe { get; set; }
        public string ImagePhotoMimeType { get; set; }
        public DateTime? AddTime { get; set; }
    }
}
