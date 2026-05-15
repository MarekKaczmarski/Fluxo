using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionCommandHandler(IFluxoDbContext context) : IDeleteTransactionCommandHandler
    {
        public async Task HandleAsync(DeleteTransactionCommand command, CancellationToken ct)
        {
            var transaction = await context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == command.Id, ct);

            if (transaction is null)
                throw new NotFoundException($"Transaction with ID {command.Id} was not found.");

            transaction.Account.RemoveTransaction(transaction);
            context.Transactions.Remove(transaction);

            await context.SaveChangesAsync(ct);
        }
    }
}
