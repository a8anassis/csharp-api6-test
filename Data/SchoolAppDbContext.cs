using Microsoft.EntityFrameworkCore;

namespace SchoolApp.Data
{
    public class SchoolAppDbContext : DbContext
    {

        public SchoolAppDbContext()
        {
        }

        public SchoolAppDbContext(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.Property(e => e.UserRole).HasConversion<string>();

                entity.Property(e => e.InsertedAt)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt)
               .ValueGeneratedOnAddOrUpdate()
               .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();
                entity.HasIndex(e => e.Username, "IX_Users_Username").IsUnique();
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teachers");

                entity.Property(e => e.InsertedAt)
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Institution, "IX_Teachers_Institution");
                entity.HasIndex(e => e.UserId, "IX_Teachers_UserId").IsUnique();
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Students");

                entity.Property(e => e.InsertedAt)
               .ValueGeneratedOnAdd()
               .HasDefaultValueSql("GETDATE()");

                entity.Property(e => e.ModifiedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("GETDATE()");

                entity.HasIndex(e => e.Institution, "IX_Students_Institution");
                entity.HasIndex(e => e.UserId, "IX_Students_UserId").IsUnique();
                entity.HasIndex(e => e.Am, "IX_Students_ΑΜ").IsUnique();
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("Courses");
                entity.HasIndex(e => e.Description, "IX_Courses_Description");                  

                entity.HasMany(e => e.Students).WithMany(s => s.Courses)
                .UsingEntity("StudentsCourses");  
            });
        }
    }
}
