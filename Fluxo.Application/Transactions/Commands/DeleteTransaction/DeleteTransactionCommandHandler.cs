using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Commands.DeleteTransaction;

public class DeleteTransactionCommandHandler(IFluxoDbContext context)
    : IDeleteTransactionCommandHandler
{
    public async Task HandleAsync(DeleteTransactionCommand command, CancellationToken ct)
    {
        var transaction =
            await context.Transactions.FirstOrDefaultAsync(t => t.Id == command.Id, ct)
            ?? throw new NotFoundException($"Transaction with ID {command.Id} was not found.");

        var account =
            await context.Accounts.FirstOrDefaultAsync(a => a.Id == transaction.AccountId, ct)
            ?? throw new NotFoundException(
                $"Account with ID {transaction.AccountId} was not found."
            );

        account.RevertTransaction(transaction.Amount, transaction.Type);

        context.Transactions.Remove(transaction);

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "The account was modified concurrently. Please retry deleting the transaction."
            );
        }
    }
}
