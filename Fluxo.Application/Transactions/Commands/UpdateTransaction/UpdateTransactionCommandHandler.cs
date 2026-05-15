using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler(
        IFluxoDbContext context,
        IValidator<UpdateTransactionCommand> validator) : IUpdateTransactionCommandHandler
    {
        public async Task HandleAsync(UpdateTransactionCommand command, CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var transaction = await context.Transactions
                .Include(t => t.Account)
                .FirstOrDefaultAsync(t => t.Id == command.Id, ct);

            if (transaction is null)
                throw new NotFoundException($"Transaction with ID {command.Id} was not found.");

            var categoryExists = await context.Categories
                .AnyAsync(c => c.Id == command.CategoryId, ct);

            if (!categoryExists)
                throw new NotFoundException($"Category with ID {command.CategoryId} was not found.");

            transaction.Account.UpdateTransaction(
                transaction,
                command.Description,
                command.Amount,
                command.Date,
                command.CategoryId,
                command.Type);

            await context.SaveChangesAsync(ct);
        }
    }
}
