using System;

namespace BLL.Interfaces.Entities
{
    public class AlbumEntity
    {
        public int Id { get; set; }
        public string AlbumName { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public DateTime? CreationTime { get; set; }
    }
}
