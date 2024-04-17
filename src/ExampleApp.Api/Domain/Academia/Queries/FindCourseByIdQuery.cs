using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record FindCourseByIdQuery(string Name, bool includeSemester = false) : IRequest<Course?>;
