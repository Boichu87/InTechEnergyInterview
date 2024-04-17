using ExampleApp.Api.Domain.Students;
using MediatR;

namespace ExampleApp.Api.Domain.Students.Queries;

internal record FindStudentsWithNumCoursesQuery() : IRequest<ICollection<Student>>;
