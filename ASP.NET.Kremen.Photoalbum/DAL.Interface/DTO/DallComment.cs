using System;

namespace DAL.Interface.DTO
{
    public class DallComment : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PhotoId { get; set; }
        public string TextComment { get; set; }
        public DateTime? CreateTime { get; set; }
    }
}
