using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler(IFluxoDbContext context) : IDeleteAccountCommandHandler
{
    public async Task HandleAsync(DeleteAccountCommand command, CancellationToken ct = default)
    {
        var account = await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == command.Id, ct);

        if (account is null)
            throw new NotFoundException($"Account with ID {command.Id} was not found.");

        var hasTransactions = await context.Transactions
            .AnyAsync(t => t.AccountId == command.Id, ct);

        if (hasTransactions)
            throw new ConflictException("Account cannot be deleted because it has transactions.");

        context.Accounts.Remove(account);
        await context.SaveChangesAsync(ct);
    }
}
