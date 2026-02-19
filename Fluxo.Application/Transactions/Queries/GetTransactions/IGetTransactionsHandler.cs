namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public interface IGetTransactionsHandler
{
    Task<IEnumerable<TransactionDto>> HandleAsync(GetTransactionsQuery query, CancellationToken ct);
}