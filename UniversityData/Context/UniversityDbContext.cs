using Microsoft.EntityFrameworkCore;
using UniversityData.ClassMappings;
using UniversityData.Entities;

namespace UniversityData.Context
{
    public class UniversityDbContext :DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CourseMapping());
            modelBuilder.ApplyConfiguration(new StudentMapping());

        }
    }
}
