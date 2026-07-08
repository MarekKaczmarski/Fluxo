using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Common;

public class AccountExistenceChecker(IFluxoDbContext context) : IAccountExistenceChecker
{
    public Task<bool> ExistsAsync(Guid id, CancellationToken ct) =>
        context.Accounts.AnyAsync(a => a.Id == id, ct);
}
