namespace ORM
{
    using System.Data.Entity;

    public partial class EntityModel : DbContext
    {
        public EntityModel()
            : base("name=EntityModel")
        {
        }

        public virtual DbSet<Album> Albums { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Photoe> Photoes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
