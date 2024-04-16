using ExampleApp.Api.Domain.Students;
using ExampleApp.Api.Domain.Students.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students.QueryHandlers;

internal class GetStudentsWithNumCoursesHandler : IRequestHandler<GetStudentsWithNumCoursesQuery, ICollection<Student>>
{
    private readonly StudentsDbContext _context;

    public GetStudentsWithNumCoursesHandler(StudentsDbContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Student>> Handle(GetStudentsWithNumCoursesQuery request, CancellationToken cancellationToken)
    {
        var results = await _context.Students
                    .Join(_context.StudentCourses, student => student.Id, studentCourse => studentCourse.StudentId, (student, studentCourse) => new { student, studentCourse })
                    .GroupBy(s => new { s.student.Id, s.student.FullName, s.student.Badge, s.student.ResidenceStatus })
                    .Select(s => new Student()
                    {
                        Id = s.Key.Id,
                        FullName = s.Key.FullName,
                        Badge = s.Key.Badge,
                        ResidenceStatus = s.Key.ResidenceStatus,
                        CourseCount = s.Count()
                    })
                    .ToListAsync(cancellationToken: cancellationToken);

        return results;
    }
}
