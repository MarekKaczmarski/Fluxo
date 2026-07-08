using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Common;

public class CategoryExistenceChecker(IFluxoDbContext context) : ICategoryExistenceChecker
{
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct) =>
        context.Categories.AnyAsync(c => c.Id == id, ct);
}
