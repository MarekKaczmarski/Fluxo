using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
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
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var account = await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == command.AccountId, ct);

        if (account is null)
            throw new NotFoundException($"Account with ID {command.AccountId} was not found.");

        var categoryExists = await context.Categories
            .AnyAsync(c => c.Id == command.CategoryId, ct);

        if (!categoryExists)
            throw new NotFoundException($"Category with ID {command.CategoryId} was not found.");

        var transaction = account.RegisterTransaction(
            Guid.NewGuid(),
            command.Amount,
            command.Description,
            command.Date,
            command.CategoryId,
            command.Type);

        context.Transactions.Add(transaction);
        await context.SaveChangesAsync(ct);

        return transaction.Id;
    }
}
