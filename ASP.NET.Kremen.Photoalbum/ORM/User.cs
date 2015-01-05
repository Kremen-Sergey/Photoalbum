namespace ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User")]
    public partial class User
    {
        public User()
        {
            Albums = new HashSet<Album>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public int RoleId { get; set; }

        public byte[] UserPhotoe { get; set; }

        [StringLength(50)]
        //[HiddenInput(DisplayValue = false)]
        public string UserPhotoMimeType { get; set; }

        public DateTime? CreationTime { get; set; }

        public DateTime? LastUpdateTime { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual Role Role { get; set; }
    }
}
