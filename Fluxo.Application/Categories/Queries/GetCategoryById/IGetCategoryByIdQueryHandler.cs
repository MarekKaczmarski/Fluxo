using Fluxo.Application.Categories.Queries.GetCategories;

namespace Fluxo.Application.Categories.Queries.GetCategoryById;

public interface IGetCategoryByIdQueryHandler
{
    Task<CategoryDto> HandleAsync(GetCategoryByIdQuery query, CancellationToken ct);
}
