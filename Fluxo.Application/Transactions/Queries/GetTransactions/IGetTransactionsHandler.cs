namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public interface IGetTransactionsHandler
{
    Task<List<TransactionDto>> Handle(GetTransactionsQuery query, CancellationToken ct);
}