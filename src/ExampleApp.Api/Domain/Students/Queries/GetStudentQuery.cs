using MediatR;

namespace ExampleApp.Api.Domain.Students.Queries;

internal record GetStudentQuery(string? FullName, string? BadgeNumber) : IRequest<Student?>;
