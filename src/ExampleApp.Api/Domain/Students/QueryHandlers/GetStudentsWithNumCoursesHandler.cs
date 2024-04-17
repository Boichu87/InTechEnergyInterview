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
        DateOnly dateOnlyCurrentVal = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
 
        var currentStudents = await _context.Students
                                .Join(_context.StudentCourses, student => student.Id, studentCourse => studentCourse.StudentId, (student, studentCourse) => new { student, studentCourse })
                                .Join(_context.Semesters, joinedData => joinedData.studentCourse.SemesterId, semester => semester.Id, (joinedData, semester) => new { student = joinedData.student, studentCourse = joinedData.studentCourse, semester })
                                .Where(data => dateOnlyCurrentVal >= data.semester.Start && dateOnlyCurrentVal <= data.semester.End)
                                .GroupBy(data => new { data.student.Id, data.student.FullName, data.student.Badge, data.student.ResidenceStatus })
                                .Select(s => new Student()
                                        {
                                            Id = s.Key.Id,
                                            FullName = s.Key.FullName,
                                            Badge = s.Key.Badge,
                                            ResidenceStatus = s.Key.ResidenceStatus,
                                            CourseCount = s.Count()
                                        }).ToListAsync(cancellationToken: cancellationToken);

        return currentStudents;
    }
}
