using ExampleApp.Api.Domain.Academia;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace ExampleApp.Api.Domain.Students;

internal class StudentCourse  
{
    public StudentCourse()
    {

    }

    [SetsRequiredMembers]
    public StudentCourse(int studentId, string courseId, string semesterId)
    {
        StudentId = studentId;
        CourseId = courseId;
        SemesterId = semesterId;
    }


    [Key, Column(Order = 0)]
    public required int StudentId { get; set; }
    public Student? Student { get; set; }

    [Key, Column(Order = 1)]
    public required string CourseId { get; set; }
    public Course? Course { get; set; }

    [Key, Column(Order = 2)]
    public required string SemesterId { get; set; } 

    public Semester? Semester { get; set; }

    public DateTimeOffset CreatedOn { get; init; }
    public DateTimeOffset LastModifiedOn { get; protected set; }
}
