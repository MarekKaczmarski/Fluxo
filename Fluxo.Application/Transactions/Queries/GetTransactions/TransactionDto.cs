using Fluxo.Domain.Enums;

namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public record TransactionDto
{
    public Guid Id { get; init; }
    public string Description { get; init; } = default!;
    public decimal Amount { get; init; }
    public DateTime Date { get; init; }
    public Guid CategoryId { get; init; }
    public string CategoryName { get; init; } = default!;
    public string Currency { get; init; } = default!;
    public TransactionType Type { get; init; }
}
