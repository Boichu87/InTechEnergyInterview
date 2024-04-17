using MediatR;

namespace ExampleApp.Api.Domain.Students.Queries;

internal record FindStudentCourseRegistrationByIdQuery(int StudentId, string CourseId, string SemesterId) : IRequest<StudentCourse?>;
