namespace Fluxo.Application.Transactions.Commands.DeleteTransaction;

public interface IDeleteTransactionCommandHandler
{
    Task HandleAsync(DeleteTransactionCommand command, CancellationToken ct);
}
