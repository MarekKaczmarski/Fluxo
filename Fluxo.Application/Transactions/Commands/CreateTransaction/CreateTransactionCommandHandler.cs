using FluentValidation;
using Fluxo.Application.Common;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Domain.Entities;
using Fluxo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler(
    IFluxoDbContext context,
    IValidator<CreateTransactionCommand> validator
) : ICreateTransactionCommandHandler
{
    public async Task<Guid> HandleAsync(CreateTransactionCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var account =
            await context.Accounts.FirstOrDefaultAsync(a => a.Id == command.AccountId, ct)
            ?? throw new NotFoundException($"Account with ID {command.AccountId} was not found.");

        var money = Money.Positive(command.Amount, account.Currency);

        var transaction = new Transaction(
            Guid.NewGuid(),
            money,
            command.Description,
            command.Date,
            command.CategoryId,
            account.Id,
            command.Type
        );

        account.ApplyTransaction(money, command.Type);

        context.Transactions.Add(transaction);

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (PostgresConstraintHelper.IsForeignKeyViolation(ex))
        {
            throw new ConflictException(
                "The referenced category no longer exists. Please retry with a valid category."
            );
        }
        catch (DbUpdateConcurrencyException)
        {
            throw new ConflictException(
                "The account was modified concurrently. Please retry the transaction."
            );
        }

        return transaction.Id;
    }
}
