using ExampleApp.Api.Domain.Students.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students.QueryHandlers;

internal class FindStudentCourseRegistrationByIdHandler : IRequestHandler<FindStudentCourseRegistrationByIdQuery, StudentCourse?>
{
    private readonly StudentsDbContext _context;

    public FindStudentCourseRegistrationByIdHandler(StudentsDbContext context)
    {
        _context = context;
    }

    public async Task<StudentCourse?> Handle(FindStudentCourseRegistrationByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.StudentCourses.Where(x => x.StudentId == request.StudentId && x.SemesterId == request.SemesterId && x.CourseId == request.CourseId).FirstOrDefaultAsync();
    }
}
