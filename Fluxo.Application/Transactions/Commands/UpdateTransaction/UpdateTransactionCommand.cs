namespace Fluxo.Application.Transactions.Commands.UpdateTransaction;

public record UpdateTransactionCommand(
    Guid Id,
    string Description,
    decimal Amount,
    DateTime Date,
    Guid CategoryId);
