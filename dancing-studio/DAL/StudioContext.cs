namespace dancing_studio.DAL
{
    using System.ComponentModel;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;

    public class StudioContext : DbContext
    {
        public StudioContext()
            : base("name=StudioContext")
        {
        }
        

        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Present> Presences { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Group>().
                HasMany(a => a.Students).
                WithMany(p => p.Groups).
                Map(
                    m =>
                    {
                        m.MapLeftKey("GroupId");
                        m.MapRightKey("StudentId");
                        m.ToTable("GrouoStudents");
                    });
            
            modelBuilder.Entity<Teacher>()
                .Property(p => p.Name)
                .IsRequired().HasMaxLength(60);

            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

        }

        public System.Data.Entity.DbSet<dancing_studio.DAL.Plan> Plans { get; set; }

    }
}