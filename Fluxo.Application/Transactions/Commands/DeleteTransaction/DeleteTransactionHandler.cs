using Fluxo.Application.Exceptions;
using Fluxo.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Application.Transactions.Commands.DeleteTransaction
{
    public class DeleteTransactionHandler(IFluxoDbContext context) : IDeleteTransactionHandler
    {
        public async Task Handle(DeleteTransactionCommand command, CancellationToken ct)
        {
            var transaction = await context.Transactions.FindAsync(command.Id);

            if (transaction is null)
            {
                throw new NotFoundException($"Transaction with ID {command.Id} was not found.");
            }

            context.Transactions.Remove(transaction);
            await context.SaveChangesAsync(ct);
        }
    }
}
