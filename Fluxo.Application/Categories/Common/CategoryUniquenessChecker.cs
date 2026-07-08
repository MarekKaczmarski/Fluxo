using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Common;

public class CategoryUniquenessChecker(IFluxoDbContext context) : ICategoryUniquenessChecker
{
    public async Task<bool> IsNameTakenAsync(string name, Guid? excludeId, CancellationToken ct)
    {
        var query = context.Categories.Where(c => c.Name == name);

        if (excludeId is Guid id)
            query = query.Where(c => c.Id != id);

        return await query.AnyAsync(ct);
    }
}
