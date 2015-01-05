namespace ORM
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Photoe")]
    public partial class Photoe
    {
        public int Id { get; set; }

        public int AlbumId { get; set; }

        public int Like { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        public byte[] ImagePhotoe { get; set; }

        [StringLength(50)]
        public string ImagePhotoMimeType { get; set; }

        public DateTime? AddTime { get; set; }

        public virtual Album Album { get; set; }
    }
}
