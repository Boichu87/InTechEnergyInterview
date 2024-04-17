using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.Academia.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class CoursesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<CoursesController> _logger;

    public CoursesController(IMediator mediator, ILogger<CoursesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

	[HttpGet(Name = "GetCurrentCoursesTask4")]
    public async Task<CurrentSemesterResponseModel> GetCurrentTask4()
    {
        DateOnly today = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
        ICollection<Course> courses = await _mediator.Send(new FindCoursesActiveOnDateQuery(today));
        _logger.LogInformation("Retrieved {Count} current courses", courses.Count);

        CurrentSemesterResponseModel model = new();

        model = courses.Select(course => new CurrentSemesterResponseModel()
        {
            Semester = new SemesterResponseModel()
            {
                StartDate = course.Semester.Start,
                EndDate = course.Semester.End,
                Key = course.Semester.Id,
                Name = course.Semester.Description,
                Courses = courses.Where(x => x.Semester.Id == course.Semester.Id).Select(x => new CourseResponseModel()
                {
                    Key = x.Id,
                    Name = x.Description,
                    Professor = new ProfessorResponseModel()
                    {
                        Key = x.Professor.Id.ToString(),
                        Name = x.Professor.FullName
                    }
                }).ToList()
            }
        }).First();

        return model;
    }

    [HttpPatch(Name = "UpdatesProfessor")]
    public async Task<ActionResult> UpdateProfessor([FromBody] ProfessorUpdateModel model)
    {
        var existingCourse = await _mediator.Send(new FindCourseByIdQuery(model.CourseId));
        if (existingCourse is null)
        {
            return NotFound($"Invalid course {model.CourseId}");
        }

        var professor = await _mediator.Send(new FindProfessorByNamedQuery(model.NewProfessorName));
        if (professor is null)
        {
            return NotFound($"Cannot file a professor named {model.NewProfessorName}");
        }

        _ = await _mediator.Send(new UpdateCourseProfessor(existingCourse.Id, professor.Id));
        return Accepted();
    }
}
