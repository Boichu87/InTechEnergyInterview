using MediatR;

namespace ExampleApp.Api.Domain.Students.Queries;

internal record FindStudentQuery(string? FullName, string? BadgeNumber) : IRequest<Student?>;
