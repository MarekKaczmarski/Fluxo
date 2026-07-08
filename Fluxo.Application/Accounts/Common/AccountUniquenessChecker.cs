using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Common;

public class AccountUniquenessChecker(IFluxoDbContext context) : IAccountUniquenessChecker
{
    public async Task<bool> IsNameTakenAsync(string name, Guid? excludeId, CancellationToken ct)
    {
        var query = context.Accounts.Where(a => a.Name == name);

        if (excludeId is Guid id)
            query = query.Where(a => a.Id != id);

        return await query.AnyAsync(ct);
    }
}
