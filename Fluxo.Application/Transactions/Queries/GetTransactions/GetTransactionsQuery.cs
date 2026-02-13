namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery();
public record TransactionDto(
    Guid Id,
    string Description,
    decimal Amount,
    DateTime Date,
    Guid CategoryId,
    Guid AccountId);