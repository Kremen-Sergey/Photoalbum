using System;
using ORM;

namespace DAL.Interface.DTO
{
    public class DalUser : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public byte[] UserPhotoe { get; set; }
        public string UserPhotoMimeType { get; set; }
        public DateTime? CreationTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public virtual Role Role { get; set; }
    }
}
