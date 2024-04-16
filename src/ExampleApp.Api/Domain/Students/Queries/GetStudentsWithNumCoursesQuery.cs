using ExampleApp.Api.Domain.Students;
using MediatR;

namespace ExampleApp.Api.Domain.Students.Queries;

internal record GetStudentsWithNumCoursesQuery() : IRequest<ICollection<Student>>;
