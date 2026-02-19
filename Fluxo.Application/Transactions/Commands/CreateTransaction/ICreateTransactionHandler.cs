using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction
{
    public interface ICreateTransactionHandler
    {
        Task<Guid> HandleAsync(CreateTransactionCommand command, CancellationToken ct);
    }
}