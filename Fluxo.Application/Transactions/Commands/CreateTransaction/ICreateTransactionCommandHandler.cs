namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public interface ICreateTransactionCommandHandler
{
    Task<Guid> HandleAsync(CreateTransactionCommand command, CancellationToken ct);
}
