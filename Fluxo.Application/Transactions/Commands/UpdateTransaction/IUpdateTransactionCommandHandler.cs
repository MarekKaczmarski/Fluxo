namespace Fluxo.Application.Transactions.Commands.UpdateTransaction;

public interface IUpdateTransactionCommandHandler
{
    Task HandleAsync(UpdateTransactionCommand command, CancellationToken ct);
}
