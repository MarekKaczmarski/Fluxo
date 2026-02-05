using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public interface ICreateTransactionHandler
{
    Task<Guid> Handle(CreateTransactionCommand command, CancellationToken ct);
}

public class CreateTransactionHandler : ICreateTransactionHandler
{
    private readonly IFluxoDbContext _context;

    public CreateTransactionHandler(IFluxoDbContext context) => _context = context;

    public async Task<Guid> Handle(CreateTransactionCommand command, CancellationToken ct)
    {
        var entity = new Transaction
        {
            Id = Guid.NewGuid(),
            Title = command.Title,
            Amount = command.Amount,
            Date = command.Date,
            Category = command.Category,
            AccountId = command.AccountId
        };

        _context.Transactions.Add(entity);
        await _context.SaveChangesAsync(ct);
        return entity.Id;
    }
}