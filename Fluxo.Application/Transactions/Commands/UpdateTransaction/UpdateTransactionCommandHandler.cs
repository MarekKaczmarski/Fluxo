using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Domain.ValueObjects;
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
                .FirstOrDefaultAsync(t => t.Id == command.Id, ct);

            if (transaction is null)
                throw new NotFoundException($"Transaction with ID {command.Id} was not found.");

            var account = await context.Accounts
                .FirstOrDefaultAsync(a => a.Id == transaction.AccountId, ct);

            if (account is null)
                throw new NotFoundException($"Account with ID {transaction.AccountId} was not found.");

            var categoryExists = await context.Categories
                .AnyAsync(c => c.Id == command.CategoryId, ct);

            if (!categoryExists)
                throw new NotFoundException($"Category with ID {command.CategoryId} was not found.");

            var oldCurrency = Currency.FromCode(transaction.Amount.Currency.Code);
            var oldMoneyForRevert = Money.Positive(transaction.Amount.Amount, oldCurrency);
            account.RevertTransaction(oldMoneyForRevert, transaction.Type);

            var newCurrencyForTx = Currency.FromCode(account.Currency.Code);
            var newMoneyForTx = Money.Positive(command.Amount, newCurrencyForTx);

            transaction.Update(
                newMoneyForTx,
                command.Description,
                command.Date,
                command.CategoryId,
                command.Type);

            var newCurrencyForAccount = Currency.FromCode(account.Currency.Code);
            var newMoneyForAccount = Money.Positive(command.Amount, newCurrencyForAccount);
            account.ApplyTransaction(newMoneyForAccount, transaction.Type);

            await context.SaveChangesAsync(ct);
        }
    }
}