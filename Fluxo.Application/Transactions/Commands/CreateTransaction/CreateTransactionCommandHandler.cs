using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler(
    IFluxoDbContext context,
    IValidator<CreateTransactionCommand> validator)
    : ICreateTransactionCommandHandler
{
    public async Task<Guid> HandleAsync(CreateTransactionCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var account = await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == command.AccountId, ct);

        if (account == null)
        {
            throw new Exception("Account not found.");
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            Amount = command.Amount,
            Description = command.Description,
            Date = command.Date,
            CategoryId = command.CategoryId,
            AccountId = command.AccountId
        };

        account.Balance += transaction.Amount;

        context.Transactions.Add(transaction);

        await context.SaveChangesAsync(ct);

        return transaction.Id;
    }
}