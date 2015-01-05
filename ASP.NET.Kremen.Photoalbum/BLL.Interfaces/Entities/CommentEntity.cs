using System;

namespace BLL.Interfaces.Entities
{
    public class CommentEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PhotoId { get; set; }
        public string TextComment { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
