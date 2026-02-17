using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler(IFluxoDbContext context) : IGetCategoriesHandler
    {
        public async Task<IEnumerable<CategoryDto>> HandleAsync(GetCategoriesQuery query, CancellationToken ct = default)
        {
            return await context.Categories
                .AsNoTracking()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Icon = c.Icon
                })
                .ToListAsync(ct);
        }
    }
}
