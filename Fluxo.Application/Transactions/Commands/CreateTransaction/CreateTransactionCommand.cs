using Fluxo.Domain.Enums;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public record CreateTransactionCommand(
    string Description,
    decimal Amount,
    DateTime Date,
    Guid CategoryId,
    Guid AccountId,
    TransactionType Type
);
