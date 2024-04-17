using System.Runtime.CompilerServices;

namespace ExampleApp.Api.Domain.Academia;

internal class Semester : ValueObject<string>
{
    public Semester(string id, string Description, DateOnly Start, DateOnly End)
    {
        this.Id = id;
        this.Description = Description;
        this.Start = Start;
        this.End = End;
    }

    public string Description { get; init; } = "Description";
    public DateOnly Start { get; init; } = default;
    public DateOnly End { get; init; } = default;
}
