using ExampleApp.Api.Domain.Academia.Commands;
using ExampleApp.Api.Domain.Students;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.CommandHandlers;

internal class RegisterStudentCourseCommandHandler : IRequestHandler<RegisterStudentCourse, Unit>
{
    private readonly AcademiaDbContext _context;
    private readonly ILogger<RegisterStudentCourseCommandHandler> _logger;

    public RegisterStudentCourseCommandHandler(
        AcademiaDbContext context,
        ILogger<RegisterStudentCourseCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<Unit> Handle(RegisterStudentCourse request, CancellationToken cancellationToken)
    {
        await _context.StudentCourses.AddAsync(new StudentCourse()
        {
            StudentId = request.StudentId,
            CourseId = request.CourseId,
            SemesterId = request.SemesterId,
            CreatedOn = DateTime.UtcNow
        });
        _logger.LogInformation(
               "Student with (id={StudentId}) added to a Course (id={CourseId}), in Semester (id={CourseId});",
                request.StudentId, request.CourseId, request.SemesterId);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
