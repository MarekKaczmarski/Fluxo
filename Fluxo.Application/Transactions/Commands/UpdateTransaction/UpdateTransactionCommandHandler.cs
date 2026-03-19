
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Application.Transactions.Commands.DeleteTransaction;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Fluxo.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler(IFluxoDbContext context) : IUpdateTransactionCommandHandler
    {
        public async Task HandleAsync(UpdateTransactionCommand command, CancellationToken ct)
        {
            var transaction = await context.Transactions.FindAsync(new object[] { command.Id }, ct);

            if (transaction is null)
            {
                throw new NotFoundException($"Transaction with ID {command.Id} was not found.");
            }

            transaction.Description = command.Description;
            transaction.Amount = command.Amount;
            transaction.Date = command.Date;
            transaction.CategoryId = command.CategoryId;

            await context.SaveChangesAsync(ct);

        }
    }
}