namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    string Title,
    decimal Amount,
    DateTime Date,
    string Category,
    Guid AccountId);