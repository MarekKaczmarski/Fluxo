using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler(IFluxoDbContext context) : IDeleteAccountCommandHandler
{
    public async Task HandleAsync(DeleteAccountCommand command, CancellationToken ct = default)
    {
        var accountExists = await context.Accounts.AnyAsync(a => a.Id == command.Id, ct);

        if (!accountExists)
            throw new NotFoundException($"Account with ID {command.Id} was not found.");

        var hasTransactions = await context.Transactions.AnyAsync(
            t => t.AccountId == command.Id,
            ct
        );

        if (hasTransactions)
            throw new ConflictException("Account cannot be deleted because it has transactions.");

        await context.Accounts.Where(a => a.Id == command.Id).ExecuteDeleteAsync(ct);
    }
}
