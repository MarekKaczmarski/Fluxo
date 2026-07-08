using FluentValidation;
using Fluxo.Application.Common;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Commands.UpdateTransaction;

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

        var oldAccount =
            await context.Accounts.FirstOrDefaultAsync(a => a.Id == transaction.AccountId, ct)
            ?? throw new NotFoundException(
                $"Account with ID {transaction.AccountId} was not found."
            );

        var newAccount =
            transaction.AccountId == command.AccountId
                ? oldAccount
                : await context.Accounts.FirstOrDefaultAsync(a => a.Id == command.AccountId, ct)
                    ?? throw new NotFoundException(
                        $"Account with ID {command.AccountId} was not found."
                    );

        oldAccount.RevertTransaction(transaction.Amount, transaction.Type);

        var newAmount = Money.Positive(command.Amount, newAccount.Currency);

        transaction.Update(
            newAmount,
            command.Description,
            command.Date,
            command.CategoryId,
            command.AccountId,
            command.Type
        );

        newAccount.ApplyTransaction(newAmount, command.Type);

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
                "The account was modified concurrently. Please retry updating the transaction."
            );
        }
    }
}
