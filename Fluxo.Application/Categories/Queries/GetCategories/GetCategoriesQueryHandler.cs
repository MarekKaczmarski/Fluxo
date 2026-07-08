using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(IFluxoDbContext context) : IGetCategoriesQueryHandler
{
    public Task<List<CategoryDto>> HandleAsync(GetCategoriesQuery query, CancellationToken ct)
    {
        return context
            .Categories.AsNoTracking()
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Icon = c.Icon,
            })
            .ToListAsync(ct);
    }
}
