namespace Fluxo.Application.Categories.Queries.GetCategories;

public record CategoryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Icon { get; init; }
}
