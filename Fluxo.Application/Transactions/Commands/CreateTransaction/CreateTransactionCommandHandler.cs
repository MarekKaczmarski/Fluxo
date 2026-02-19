using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandHandler(
    IFluxoDbContext context,
    IValidator<CreateTransactionCommand> validator)
    : ICreateTransactionHandler
{
    public async Task<Guid> HandleAsync(CreateTransactionCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var entity = new Transaction
        {
            Id = Guid.NewGuid(),
            Description = command.Description,
            Amount = command.Amount,
            Date = command.Date,
            CategoryId = command.CategoryId,
            AccountId = command.AccountId
        };

        context.Transactions.Add(entity);
        await context.SaveChangesAsync(ct);

        return entity.Id;
    }
}