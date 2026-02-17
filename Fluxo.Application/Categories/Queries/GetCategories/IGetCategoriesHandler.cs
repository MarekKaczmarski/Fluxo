namespace Fluxo.Application.Categories.Queries.GetCategories
{
    public interface IGetCategoriesHandler
    {
        Task<IEnumerable<CategoryDto>> HandleAsync(GetCategoriesQuery query, CancellationToken ct = default);
    }
}
