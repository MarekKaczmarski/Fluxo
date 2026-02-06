using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionHandler(
    IFluxoDbContext context,
    IValidator<CreateTransactionCommand> validator)
    : ICreateTransactionHandler
{
    public async Task<Guid> Handle(CreateTransactionCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var entity = new Transaction
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Amount = command.Amount,
            Date = command.Date,
            Category = command.Category,
            AccountId = command.AccountId
        };

        context.Transactions.Add(entity);
        await context.SaveChangesAsync(ct);

        return entity.Id;
    }
}