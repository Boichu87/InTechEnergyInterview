namespace ExampleApp.Api.Domain.Academia;

internal class Professor : ValueObject<int>
{

    public Professor()
    {
        
    }

    public Professor(int Id, string FullName)
    {
        this.Id = Id;
        this.FullName = FullName;
    }

    public string FullName { get; init; } = "TBD";
    public string? Extension { get; init; }
}
