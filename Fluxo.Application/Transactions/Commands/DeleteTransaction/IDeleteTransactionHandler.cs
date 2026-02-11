namespace Fluxo.Application.Transactions.Commands.DeleteTransaction
{
    public interface IDeleteTransactionHandler
    {
        Task Handle(DeleteTransactionCommand command, CancellationToken ct);
    }
}