using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleApp.Api.Domain.Students;

internal class Student : AggregateRoot<int>
{
    public required string FullName { get; set; }
    public required string Badge { get; set; }
    public required string ResidenceStatus { get; set; }
    public ICollection<StudentCourse>? StudentCourses { get; set; }

    public Student() { }

    protected Student(int id, string fullName, string badge, string residenceStatus, DateTimeOffset createdOn, DateTimeOffset lastModifiedOn)
    {
        Id = id;
        FullName = fullName;
        Badge = badge;
        ResidenceStatus = residenceStatus;
        CreatedOn = createdOn;
        LastModifiedOn = lastModifiedOn;
    }

    [NotMapped]
    public int CourseCount { get; set; }
}
