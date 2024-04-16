using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Domain.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IMediator mediator, ILogger<StudentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet(Name = "GetCurrentStudentsWithNumCourses")]
    public async Task<IEnumerable<StudentModel>> GetCurrentStudentsWithNumCourses()
    {
        ICollection<Student> students = await _mediator.Send(new GetStudentsWithNumCoursesQuery());
        _logger.LogInformation("Retrieved {Count} current students", students.Count);

        List <StudentModel> models = new();
        foreach (var student in students)
        {
           StudentModel studentModel = new(student.Id, student.FullName, student.Badge, student.ResidenceStatus, student.CourseCount);
            models.Add(studentModel);
        }

        return models;
    }
}
