namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public interface IGetTransactionsQueryHandler
{
    Task<IEnumerable<TransactionDto>> HandleAsync(GetTransactionsQuery query, CancellationToken ct);
}