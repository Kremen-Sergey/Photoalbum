namespace ORM
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<User>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string RoleName { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
