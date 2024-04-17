namespace ExampleApp.Api.Controllers.Models;

public record StudentRegisterModel
{
    public  string? FullName { get; init; }
    public  string? BadgeNumber { get; init; }
    public required string CourseId { get; init; }
};
