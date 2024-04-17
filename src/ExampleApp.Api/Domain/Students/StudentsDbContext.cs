using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Extensions;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students;

internal class StudentsDbContext : DbContext
{
    public StudentsDbContext(DbContextOptions<StudentsDbContext> options) : base(options)
    {
    }
 
    internal DbSet<Student> Students { get; set; }
    internal DbSet<StudentCourse> StudentCourses { get; set; }
    internal DbSet<Semester> Semesters { get; set; }

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
