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
[Route("[controller]/[action]")]
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
        ICollection<Student> students = await _mediator.Send(new FindStudentsWithNumCoursesQuery());
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
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status422UnprocessableEntity)]
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

        Student? student = await _mediator.Send(new FindStudentQuery(model.FullName, model.BadgeNumber));

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

        StudentCourse? studentCourse = await _mediator.Send(new FindStudentCourseRegistrationByIdQuery(student.Id, model.CourseId, existingCourse.Semester.Id));

        if (studentCourse != null)
        {
            return UnprocessableEntity($"The Student is already registered for the given Course.");
        }
        #endregion

         _ = await _mediator.Send(new RegisterStudentCourse(existingCourse.Id, student.Id, existingCourse.Semester.Id));
        ApiResponseModel<string> response = new ApiResponseModel<string>();
        response.StatusCode = HttpStatusCode.Created;
        response.Message = "Student successfuly registered.";
        response.Success = true;
        return response;

        }
        catch (Exception ex)
        {
            //NOTE: Any kind of Exception handling was not required in tasks but added here at least, just in case.
            ApiResponseModel<string> response = new ApiResponseModel<string>();
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.ErrorMessage = ex.Message;
            response.Success = false;
            return response;
        }
    }

    /// <summary>
    /// /TASK 3
    ///Building upon the previous tasks, add an end-point to allow a Student to un-register for a Course.
    ///It's entirely up to you if you want to handle this operation as a "soft-" or "hard-delete".

    ///The minimum payload sent to this new end-point should have either the Student's FullName or their Badge "number", and the Id of the Course they registered for.
    ///Validate that the Course is not "past".
    ///A Student cannot un-register from a past course; produce an appropriate error message when this situation occurs.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>

    [HttpPost(Name = "UnRegisterStudent")]
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult> UnRegisterStudent([FromBody] StudentRegisterModel model)
    {
        try
        {
            #region TASK3-Validations
            if (string.IsNullOrEmpty(model.CourseId))
            {
                return UnprocessableEntity($"A Course is required.");
            }

            if (string.IsNullOrEmpty(model.BadgeNumber) && string.IsNullOrEmpty(model.FullName))
            {
                return UnprocessableEntity($"At least BadgeNumber or FullName must be provided for Student registration.");
            }

            Student? student = await _mediator.Send(new FindStudentQuery(model.FullName, model.BadgeNumber));

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

            if (currentdate < existingCourse.Semester.Start && currentdate < existingCourse.Semester.End)
            {
                return UnprocessableEntity($"The Course attempted to un-register that is 'Past'.");
            }

            StudentCourse? studentCourse = await _mediator.Send(new FindStudentCourseRegistrationByIdQuery(student.Id, model.CourseId, existingCourse.Semester.Id));
  
            if (studentCourse == null)
            {
                return UnprocessableEntity($"The Student is not registered for the given Course.");
            }
            #endregion

            _ = await _mediator.Send(new RegisterStudentCourse(existingCourse.Id, student.Id, existingCourse.Semester.Id));
            ApiResponseModel<string> response = new ApiResponseModel<string>();
            response.StatusCode = HttpStatusCode.Accepted;
            response.Message = "Student successfuly un-registered.";
            response.Success = true;
            return response;

        }
        catch (Exception ex)
        {
            //NOTE: Any kind of Exception handling was not required in tasks but added here at least, just in case.
            ApiResponseModel<string> response = new ApiResponseModel<string>();
            response.StatusCode = HttpStatusCode.InternalServerError;
            response.ErrorMessage = ex.Message;
            response.Success = false;
            return response;
        }
    }



    /// <summary>
    /// Mock  endpoint for upload a file in order to create a batch students registration feature for BONUS TASK 6
    /// </summary>
    /// <param name="importType">Could be a user defined type from a defined string group (xls, pdf, csv)</param>
    /// <param name="formFile">The file to be uploaded (Restriction in the type must be included of course, both in back-end or the consumer client</param>
    /// <returns></returns>
    [HttpPost("{importType}")]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ApiResponseModel<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchRegistration(string importType, IFormFile formFile)
    {
        //TASK 6
        //As a new school year rolls by, the admin staff would like to be able to upload the student registration in bulk.

        //They have a spreadsheet with a large number of elections.
        //The spreadsheet has three columns:

        //Student Name
        //Student Badge
        //Course Id
        //Due to the very large size of this data set, EF exhibits poor performance creating the records.

        //Provide a technical solution that ensures this large amount of data is inserted as fast as possible.

        //Notes:

        //the entry-point should still be the API: an end-point that accepts an Excel file upload --or CSV, and which then parses and loads the data therein into the database;
        //        the solution should be entirely contained within the ExampleApp.Api project; you don't have access to the bulk-load tools provided by osql or similar command-line tools;
        //discuss how this would change if the application was deployed to a cloud provider like Azure or AWS: what services would you use then?
        //Setting Up


        ///MY ANSWER FOR THIS
        ///DIFICULT ANSWER WITH SO LITTLE INFO, BUT I WILL ASSUME A LOT FOR THE SAKE OF AN ANSWER.
        ///We can avoid EF completely if ExecuteUpdate (currently in EF 7 is not fast enough)
        ///Use a transaction with a sql MERGE within (or not) in the app (not the best but still is in the project) against an Origin temp table (or a regular teable for this purpose with a proper tracking state columns for this process)  with the values to import,
        ///For that Origin table using their badge as an improvised join ID culd be a posssibility,
        ///if the badge doesnt exist in the Target table of StudentCourse (already joined with student for the course in order th check their Badges, supossing those are Unique), and is in the current semester (asumming that with other validations) just insert the record.
        ///Of course that solution is not the best for track, you will require extra logic and effort thtat will consume development effort, testing, maintanance, for keep track of the inserted bulk or if there were other issue, for example, a failure in the middle of the process. Which strategy to use in there, let's say, is out of scope
        ///Considering we are not using the EF at all. Just classic bulk insert/update/delete strategies.
        ///

        ///Another solution (the more complex a probably more expensive in a cloud environment)
        ///Just save the excel file in a blob storage or directly to a table in the db for this purpose and process it in the background with other service using sql transactions
        ///For example, after persisting the file in the format defined, set an Azure Service Bus Queue entry and with other App service trough an Azure function to take that entry as an In Trigger, and trough it process it with a Receiver (worker, appService, or whatever we want to build) service using raw sql transactions with SQL MERGE for example to achieve the goals. (and with the proper rollback scenario, even lock features at data level until the process is properly completed).
        ///After this is finished or not, do the proper queue handling and notificate back to the Sender Service (if this service is SPA app or another service just push the proper notification).


        ///Both scenarios with their proper validations for content, file-type, size, etc.

        return null;
    }
}
