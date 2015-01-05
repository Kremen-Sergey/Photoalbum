using System;

namespace DAL.Interface.DTO
{
    public class DalAlbum : IEntity
    {
        public int Id { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime? CreationTime { get; set; }
    }
}
