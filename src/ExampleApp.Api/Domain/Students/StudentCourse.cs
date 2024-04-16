using ExampleApp.Api.Domain.Academia;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExampleApp.Api.Domain.Students;

internal class StudentCourse  
{
    [Key, Column(Order = 0)]
    public int StudentId { get; set; }
    public required Student Student { get; set; } // Navigation property to Student

    [Key, Column(Order = 1)]
    public string CourseId { get; set; }
    public required Course Course { get; set; } // Navigation property to Course

    [Key, Column(Order = 2)]
    public string SemesterId { get; set; } // Semester the student is registered

    public required Semester Semester { get; set; } // Navigation property to Course

    public DateTimeOffset CreatedOn { get; init; }
    public DateTimeOffset LastModifiedOn { get; protected set; }
}
