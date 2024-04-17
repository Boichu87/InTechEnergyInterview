using MediatR;

namespace ExampleApp.Api.Domain.Academia.Commands;

internal record RegisterStudentCourse(string CourseId, int StudentId, string SemesterId) : IRequest<Unit>;
