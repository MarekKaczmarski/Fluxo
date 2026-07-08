namespace Fluxo.Application.Categories.Queries.GetCategories;

public interface IGetCategoriesQueryHandler
{
    Task<List<CategoryDto>> HandleAsync(GetCategoriesQuery query, CancellationToken ct);
}
