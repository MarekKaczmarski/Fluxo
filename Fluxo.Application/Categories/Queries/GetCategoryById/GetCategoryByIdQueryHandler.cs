using Fluxo.Application.Categories.Queries.GetCategories;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Queries.GetCategoryById;

public class GetCategoryByIdQueryHandler(IFluxoDbContext context) : IGetCategoryByIdQueryHandler
{
    public async Task<CategoryDto> HandleAsync(GetCategoryByIdQuery query, CancellationToken ct)
    {
        return await context
                .Categories.AsNoTracking()
                .Where(c => c.Id == query.Id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Icon = c.Icon,
                })
                .FirstOrDefaultAsync(ct)
            ?? throw new NotFoundException($"Category with ID {query.Id} was not found.");
    }
}
