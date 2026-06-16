using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Domain.Entities;
using Fluxo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction
{
    public class CreateTransactionCommandHandler(
        IFluxoDbContext context,
        IValidator<CreateTransactionCommand> validator) : ICreateTransactionCommandHandler
    {
        public async Task<Guid> HandleAsync(CreateTransactionCommand command, CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var account = await context.Accounts
                .FirstOrDefaultAsync(a => a.Id == command.AccountId, ct);

            if (account is null)
                throw new NotFoundException($"Account with ID {command.AccountId} was not found.");

            var categoryExists = await context.Categories
                .AnyAsync(c => c.Id == command.CategoryId, ct);

            if (!categoryExists)
                throw new NotFoundException($"Category with ID {command.CategoryId} was not found.");

            var transactionId = Guid.NewGuid();

            var currencyForTransaction = Currency.FromCode(account.Currency.Code);
            var moneyForTransaction = Money.Positive(command.Amount, currencyForTransaction);

            var transaction = new Transaction(
                transactionId,
                moneyForTransaction,
                command.Description,
                command.Date,
                command.CategoryId,
                account.Id,
                command.Type);

            var currencyForAccountUpdate = Currency.FromCode(account.Currency.Code);
            var moneyForAccountUpdate = Money.Positive(command.Amount, currencyForAccountUpdate);

            account.ApplyTransaction(moneyForAccountUpdate, command.Type);

            await context.Transactions.AddAsync(transaction, ct);
            await context.SaveChangesAsync(ct);

            return transaction.Id;
        }
    }
}