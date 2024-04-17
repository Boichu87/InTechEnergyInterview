using ExampleApp.Api.Domain.Students.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExampleApp.Api.Domain.Students.QueryHandlers;

internal class FindStudentHandler : IRequestHandler<FindStudentQuery, Student?>
{
    private readonly StudentsDbContext _context;

    public FindStudentHandler(StudentsDbContext context)
    {
        _context = context;
    }

    public async Task<Student?> Handle(FindStudentQuery request, CancellationToken cancellationToken)
    {
        return await _context.Students.Where(x => ( !string.IsNullOrEmpty(request.FullName) && x.FullName == request.FullName) || (!string.IsNullOrEmpty(request.BadgeNumber) && x.Badge == request.BadgeNumber)).FirstOrDefaultAsync();
    }
}
