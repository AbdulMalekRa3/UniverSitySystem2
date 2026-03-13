using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityData.ClassMappings;
using UniversityData.Entities;

namespace UniversityData.Context
{
    public class UniversityDbContext : IdentityDbContext<
    User,
    Role,
    int,
    UserClaim,
    UserRole,
    UserLogin,
    RoleClaim,
    UserToken>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        public UniversityDbContext(DbContextOptions<UniversityDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new RoleMapping());
            //modelBuilder.ApplyConfiguration(new UserClaimMapping());
            //modelBuilder.ApplyConfiguration(new UserRoleMapping());
            //modelBuilder.ApplyConfiguration(new UserLoginMapping());
            //modelBuilder.ApplyConfiguration(new RoleClaimMapping());
            //modelBuilder.ApplyConfiguration(new UserTokenMapping());   
            //modelBuilder.ApplyConfiguration(new UserMapping());
            modelBuilder.ApplyConfiguration(new CourseMapping());
            modelBuilder.ApplyConfiguration(new StudentMapping());

        }
    }
}
