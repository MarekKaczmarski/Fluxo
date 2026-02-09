namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public record GetTransactionsQuery();
public record TransactionDto(
    Guid Id,
    string Title,
    decimal Amount,
    DateTime Date,
    string Category);