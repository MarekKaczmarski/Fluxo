namespace Fluxo.Application.Transactions.Commands.DeleteTransaction
{
    public interface IDeleteTransactionHandler
    {
        Task HandleAsync(DeleteTransactionCommand command, CancellationToken ct);
    }
}