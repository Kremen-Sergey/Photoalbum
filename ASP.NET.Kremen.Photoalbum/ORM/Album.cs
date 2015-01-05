namespace ORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Album")]
    public partial class Album
    {
        public Album()
        {
            Photoes = new HashSet<Photoe>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string AlbumName { get; set; }

        public string Description { get; set; }

        public int UserId { get; set; }

        public DateTime? CreationTime { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Photoe> Photoes { get; set; }
    }
}
