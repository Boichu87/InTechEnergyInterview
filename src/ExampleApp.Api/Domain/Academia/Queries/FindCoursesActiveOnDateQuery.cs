using MediatR;

namespace ExampleApp.Api.Domain.Academia.Queries;

internal record FindCoursesActiveOnDateQuery(DateOnly ActiveOn) : IRequest<ICollection<Course>>;
