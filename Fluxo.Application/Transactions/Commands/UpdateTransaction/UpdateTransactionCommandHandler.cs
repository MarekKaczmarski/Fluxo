using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandHandler(
        IFluxoDbContext context,
        IValidator<UpdateTransactionCommand> validator
    ) : IUpdateTransactionCommandHandler
    {
        public async Task HandleAsync(UpdateTransactionCommand command, CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(command, ct);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var transaction =
                await context.Transactions.FirstOrDefaultAsync(t => t.Id == command.Id, ct)
                ?? throw new NotFoundException($"Transaction with ID {command.Id} was not found.");

            var account =
                await context.Accounts.FirstOrDefaultAsync(a => a.Id == transaction.AccountId, ct)
                ?? throw new NotFoundException(
                    $"Account with ID {transaction.AccountId} was not found."
                );

            var categoryExists = await context.Categories.AnyAsync(
                c => c.Id == command.CategoryId,
                ct
            );

            if (!categoryExists)
            {
                throw new NotFoundException(
                    $"Category with ID {command.CategoryId} was not found."
                );
            }

            account.RevertTransaction(transaction.Amount, transaction.Type);

            var newAmount = Money.Positive(command.Amount, account.Currency);

            transaction.Update(
                newAmount,
                command.Description,
                command.Date,
                command.CategoryId,
                command.Type
            );
            account.ApplyTransaction(newAmount, command.Type);

            try
            {
                await context.SaveChangesAsync(ct);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw new ConflictException(
                    "The account was modified concurrently. Please retry updating the transaction."
                );
            }
        }
    }
}
