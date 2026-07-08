namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public interface IGetTransactionsQueryHandler
{
    Task<List<TransactionDto>> HandleAsync(GetTransactionsQuery query, CancellationToken ct);
}
