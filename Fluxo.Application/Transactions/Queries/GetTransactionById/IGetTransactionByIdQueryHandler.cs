using Fluxo.Application.Transactions.Queries.GetTransactions;

namespace Fluxo.Application.Transactions.Queries.GetTransactionById;

public interface IGetTransactionByIdQueryHandler
{
    Task<TransactionDto> HandleAsync(GetTransactionByIdQuery query, CancellationToken ct);
}
