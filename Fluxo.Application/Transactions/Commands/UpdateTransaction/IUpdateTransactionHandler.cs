using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Application.Transactions.Commands.UpdateTransaction
{
    public interface IUpdateTransactionHandler
    {
        Task HandleAsync(UpdateTransactionCommand command, CancellationToken ct);
    }
}
