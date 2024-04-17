using ExampleApp.Api.Controllers;
using Microsoft.Extensions.Logging;
using ExampleApp.Api.Domain.Academia;
using FluentAssertions;

namespace ExampleApp.Tests;

public class CoursesControllerTests
{
    private readonly IMediator _mediator;
    private readonly ILogger<CoursesController> _logger = Utils.CreateLogger<CoursesController>();

    public CoursesControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [Fact]
    public async Task MapsCurrentCourses()
    {
        // Arrange
        List<Course> courses = new()
        {
            new Course(
                "test1",
                "test 1",
                new Semester("sem1","sem-1", DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddMonths(9))),
                new Professor(1, "prof one"),
                DateTimeOffset.Now,
                DateTimeOffset.Now
            ),
            new Course(
                "test2",
                "test 2",
                new Semester("sem1","sem-1", DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddMonths(9))),
                new Professor(2, "prof two"),
                DateTimeOffset.Now,
                DateTimeOffset.Now
            )
        };
        _mediator.Send(Arg.Any<IRequest<ICollection<Course>>>())
            .Returns(courses);

        // Act
        var response = await new CoursesController(_mediator, _logger).GetCurrentTask4();

        // Assert
        response.Should()
            .BeEquivalentTo(
                new
                {
                   Semester = new
                   {
                       Key = "sem1",
                       Name = "sem-1",
                       StartDate = DateOnly.FromDateTime(DateTime.Today),
                       EndDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(9)),
                       Courses = new[] {
                           new {
                               Key = "test1",
                               Name = "test 1",
                               Professor =  new {
                                    Key = "1",
                                    Name =  "prof one"
                                }       
                           },
                           new {
                               Key = "test2",
                               Name = "test 2",
                               Professor =  new {
                                    Key = "2",
                                    Name =  "prof two"
                                }
                           }
                       }
                   }
                });
    }
}
