using ExampleApp.Api.Controllers;
using Microsoft.Extensions.Logging;
using ExampleApp.Api.Domain.Academia;
using FluentAssertions;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Controllers.Models;

namespace ExampleApp.Tests;

public class StudentControllerTests
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentsController> _logger = Utils.CreateLogger<StudentsController>();

    public StudentControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [Fact]
    public async Task MapsCurrentCourses()
    {
        // Arrange
        List<Student> students = new()
        {
            new Student()
            {
                Id = 1,
                FullName = "Player One",
                Badge = "player-one",
                ResidenceStatus = "InState",
                CourseCount = 2
            },
            new Student()
            {
                Id = 2,
                FullName = "Santa I Claus",
                Badge = "santa-i-claus",
                ResidenceStatus = "Foreign",
                CourseCount = 2
            },
            new Student()
            {
                Id = 3,
                FullName = "Alf",
                Badge = "alf",
                ResidenceStatus = "OutOfState",
                CourseCount = 1
            }
        };

        _mediator.Send(Arg.Any<IRequest<ICollection<Student>>>())
            .Returns(students);

        // Act
        var response = await new StudentsController(_mediator, _logger).GetCurrentStudentsWithNumCourses();

        // Assert
        response.Should().HaveCount(3);
        response.Should()
            .BeEquivalentTo(
                new[]
                {
                    new StudentModel()
                    {
                            Id = 1,
                            FullName = "Player One",
                            Badge = "player-one",
                            ResidenceStatus = "InState",
                            CourseCount = 2
                      },
                      new StudentModel
                      {
                            Id = 2,
                            FullName = "Santa I Claus",
                            Badge = "santa-i-claus",
                            ResidenceStatus = "Foreign",
                            CourseCount = 2
                      },
                      new StudentModel
                      {
                            Id = 3,
                            FullName = "Alf",
                            Badge = "alf",
                            ResidenceStatus = "OutOfState",
                            CourseCount = 1
                      }
                });
    }
}
