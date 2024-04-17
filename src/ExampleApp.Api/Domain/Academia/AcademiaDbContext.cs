using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Academia;

internal class AcademiaDbContext : DbContext
{
    public AcademiaDbContext(DbContextOptions<AcademiaDbContext> options) : base(options)
    {
    }

    internal DbSet<Course> Courses { get; set; }
    internal DbSet<Professor> Professors { get; set; }
    internal DbSet<Semester> Semesters { get; set; }
    internal DbSet<StudentCourse> StudentCourses { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Semester>(
            e =>
            {
                e.Property(x => x.Start).HasColumnName("StartDate");
                e.Property(x => x.End).HasColumnName("EndDate");
            });

        builder.Entity<StudentCourse>()
            .HasKey(e => new { e.StudentId, e.CourseId, e.SemesterId });
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        base.ConfigureConventions(builder);

        builder.Properties<DateOnly>()
            .HaveConversion<DateOnlyConverter>()
            .HaveColumnType("date");
    }
}
