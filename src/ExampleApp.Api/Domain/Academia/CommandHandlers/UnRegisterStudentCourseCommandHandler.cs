using ExampleApp.Api.Domain.Academia.Commands;
using MediatR;

namespace ExampleApp.Api.Domain.Academia.CommandHandlers;

internal class UnRegisterStudentCourseCommandHandler : IRequestHandler<RegisterStudentCourse, Unit>
{
    private readonly AcademiaDbContext _context;
    private readonly ILogger<RegisterStudentCourseCommandHandler> _logger;

    public UnRegisterStudentCourseCommandHandler(
        AcademiaDbContext context,
        ILogger<RegisterStudentCourseCommandHandler> logger)
    {
        _context = context;
        _logger = logger;
    }
    public async Task<Unit> Handle(RegisterStudentCourse request, CancellationToken cancellationToken)
    {
        /// Note: Task said It's entirely up to you if you want to handle this operation as a "soft-" or "hard-delete".. So id decided to do a Hard Delete just for not waste too much time
        /// in db modifications for included soft deleted flag/logic without migrations in this excercise.
        var registration = _context.StudentCourses.Where(x=> x.CourseId == request.CourseId && x.StudentId == request.StudentId).FirstOrDefault();

        if(registration != null)
        {
            _context.StudentCourses.Remove(registration);
        }

        _logger.LogInformation(
               "Student with (id={StudentId}) removed from Course (id={CourseId})",
                request.StudentId, request.CourseId);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
