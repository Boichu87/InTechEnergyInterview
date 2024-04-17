using ExampleApp.Api.Controllers;
using Microsoft.Extensions.Logging;
using ExampleApp.Api.Domain.Academia;
using FluentAssertions;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Controllers.Models;
using static ExampleApp.Api.Controllers.StudentsController;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students.Queries;
using Microsoft.AspNetCore.Mvc;
using NSubstitute.ReturnsExtensions;

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
    public async Task MapCurrentStudentsWithNumCourses()
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

    [Fact]
    public async Task Task2_RegisterStudentToCourse_Ok()
    {
        //Arrange
        StudentRegisterModel studentToRegister = new StudentRegisterModel()
        {
            BadgeNumber = "101",
            CourseId = "TEST",
            FullName = "John Doe"
        };

        Student student = new Student()
        {
            Id = 1,
            FullName = "John Doe",
            Badge = "101",
            ResidenceStatus = "InState"
        };

        Semester semester = new Semester() { Start = DateOnly.FromDateTime(DateTime.UtcNow), End = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(9)) };
        Professor professor =  new Professor() { FullName = "Test Jones"};
        Course courseToRegister = new Course("TEST", "TEST course", semester, professor, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

        _mediator.Send(new FindCourseByIdQuery(studentToRegister.CourseId, includeSemester: true)).Returns(courseToRegister);
        _mediator.Send(new GetStudentQuery(studentToRegister.FullName, studentToRegister.BadgeNumber)).Returns(student);

        // Act
        var response = await new StudentsController(_mediator, _logger).RegisterStudent(studentToRegister);


        // Assert
        response.Should().BeEquivalentTo(new ApiResponse<string> { StatusCode = System.Net.HttpStatusCode.Created, Success = true, Message = "Student successfuly registered." });
    }

    [Fact]
    public async Task Task2_RegisterStudentToCourse_InvalidParameters()
    {
        //Arrange
        StudentRegisterModel studentToRegister = new StudentRegisterModel()
        {
            BadgeNumber = null,
            CourseId = "TEST",
            FullName = null
        };

        Student student = new Student()
        {
            Id = 1,
            FullName = "John Doe",
            Badge = "101",
            ResidenceStatus = "InState"
        };

        Semester semester = new Semester() { Start = DateOnly.FromDateTime(DateTime.UtcNow), End = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(9)) };
        Professor professor = new Professor() { FullName = "Test Jones" };
        Course courseToRegister = new Course("TEST", "TEST course", semester, professor, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);

        _mediator.Send(new FindCourseByIdQuery(studentToRegister.CourseId, includeSemester: true)).Returns(courseToRegister);
        _mediator.Send(new GetStudentQuery(studentToRegister.FullName, studentToRegister.BadgeNumber)).Returns(student);

        // Act
        var response = await new StudentsController(_mediator, _logger).RegisterStudent(studentToRegister);

        // Assert
        response.Should().BeEquivalentTo(new UnprocessableEntityObjectResult("At least BadgeNumber or FullName must be provided for Student registration.") { StatusCode = 422 });
    }

    [Fact]
    public async Task Task2_RegisterStudentToCourse_CourseNotFound()
    {
        //Arrange
        StudentRegisterModel studentToRegister = new StudentRegisterModel()
        {
            BadgeNumber = "101",
            CourseId = "TEST",
            FullName = "John Doe"
        };

        Student student = new Student()
        {
            Id = 1,
            FullName = "John Doe",
            Badge = "101",
            ResidenceStatus = "InState"
        };

        Semester semester = new Semester() { Start = DateOnly.FromDateTime(DateTime.UtcNow), End = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(9)) };
        Professor professor = new Professor() { FullName = "Test Jones" };


        _mediator.Send(new FindCourseByIdQuery(studentToRegister.CourseId, includeSemester: true)).ReturnsNull();
        _mediator.Send(new GetStudentQuery(studentToRegister.FullName, studentToRegister.BadgeNumber)).Returns(student);

        // Act
        var response = await new StudentsController(_mediator, _logger).RegisterStudent(studentToRegister);


        // Assert
        response.Should().BeEquivalentTo(new NotFoundObjectResult("Invalid Course TEST") { StatusCode = 404 });
    }

    [Fact]
    public async Task Task2_RegisterStudentToCourse_StudentNotFound()
    {
        //Arrange
        StudentRegisterModel studentToRegister = new StudentRegisterModel()
        {
            BadgeNumber = "101",
            CourseId = "TEST",
            FullName = "John Doe"
        };

        Semester semester = new Semester() { Start = DateOnly.FromDateTime(DateTime.UtcNow), End = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(9)) };
        Professor professor = new Professor() { FullName = "Test Jones" };

        _mediator.Send(new GetStudentQuery(studentToRegister.FullName, studentToRegister.BadgeNumber)).ReturnsNull();

        // Act
        var response = await new StudentsController(_mediator, _logger).RegisterStudent(studentToRegister);


        // Assert
        response.Should().BeEquivalentTo(new NotFoundObjectResult("Student Not Found.") { StatusCode = 404 });
    }


}
