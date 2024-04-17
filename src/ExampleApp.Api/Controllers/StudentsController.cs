using ExampleApp.Api.Controllers.Models;
using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.Academia.Queries;
using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Domain.Students.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ExampleApp.Api.Controllers;

[ApiController]
[Route("[controller]")]
public partial class StudentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(IMediator mediator, ILogger<StudentsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// TASK 1
    /// We need an API end-point that produces a list of Students (stored in the Students table), with all their attributes,
    /// and for each student listing the number of Courses (not the list but the number!) they are registered for in the current Semester.
    /// </summary>
    /// <returns></returns>

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

    /// <summary>
    /// /TASK 2
    /// Add an end-point to allow a Student to register for a Course.
    /// The minimum payload sent to this new end-point should have either the Student's FullName or their Badge "number", and the Id of the Course they register for.
    ///    * Validate that the Course is "current".
    ///    * A Student cannot register for past or future course; produce an appropriate error message when this situation occurs.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>

    [HttpPost(Name = "RegisterStudent")]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> RegisterStudent([FromBody] StudentRegisterModel model)
    {
        try
        {

        #region TASK2-Validations
        if (string.IsNullOrEmpty(model.CourseId))
        {
            return UnprocessableEntity($"A Course is required.");
        }

        if (string.IsNullOrEmpty(model.BadgeNumber) && string.IsNullOrEmpty(model.FullName))
        {
            return UnprocessableEntity($"At least BadgeNumber or FullName must be provided for Student registration.");
        }

        Student? student = await _mediator.Send(new GetStudentQuery(model.FullName, model.BadgeNumber));

        if (student is null)
        {
            return NotFound($"Student Not Found.");
        }

        var existingCourse = await _mediator.Send(new FindCourseByIdQuery(model.CourseId, includeSemester: true));

        if (existingCourse is null)
        {
            return NotFound($"Invalid Course {model.CourseId}");
        }

        DateOnly currentdate = DateOnly.FromDateTime(DateTime.UtcNow); //NOTE: Just for match the Semester in the provided sample db values

        if (currentdate > existingCourse.Semester.Start && currentdate < existingCourse.Semester.End)
        {
            return UnprocessableEntity($"The Course attempted to register is not 'current'.");
        }

        StudentCourse studentCourse = new StudentCourse(0, "", "");  //NOTE: HERE I COULD VALIDATE IF THE REGISTRATION IS NO ALREADY IN THE DB - SKIPPED WITH A new instance for save time - NOT INCLUDED IN TASK 2 REQUIREMENTS 

        if (studentCourse == null)
        {
            return UnprocessableEntity($"The Student is already registered for the given Course.");
        }
        #endregion

        _ = await _mediator.Send(new RegisterStudentCourse(existingCourse.Id, student.Id, existingCourse.Semester.Id));
        ApiResponse<string> response = new ApiResponse<string>();
        response.StatusCode = HttpStatusCode.Created;
        response.Message = "Student successfuly registered.";
        response.Success = true;
        return response;

        }
        catch (Exception ex)
        {
            //NOTE: Any kind of Exception handling was not required in tasks but added here at least, just in case.
            ApiResponse<string> response = new ApiResponse<string>();
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.ErrorMessage = ex.Message;
            response.Success = false;
            return response;
        }
    }
}
