namespace ExampleApp.Api.Domain;

internal class AggregateRoot<T> where T : notnull
{
    public T Id { get; init; } = default!;
    public DateTimeOffset CreatedOn { get; init; }
    public DateTimeOffset LastModifiedOn { get; protected set; }
}
