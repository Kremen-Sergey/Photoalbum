namespace ORM
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Comment")]
    public partial class Comment
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int PhotoId { get; set; }

        [Required]
        public string TextComment { get; set; }

        public DateTime? CreateTime { get; set; }
    }
}
