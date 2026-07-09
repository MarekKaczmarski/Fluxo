using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Fluxo.Application.Transactions.Commands.DeleteTransaction;
using Fluxo.Application.Transactions.Commands.UpdateTransaction;
using Fluxo.Application.Transactions.Queries.GetTransactionById;
using Fluxo.Application.Transactions.Queries.GetTransactions;
using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Controllers;

[Route("api/[controller]")]
public class TransactionsController(
    IGetTransactionsQueryHandler getHandler,
    IGetTransactionByIdQueryHandler getByIdHandler,
    ICreateTransactionCommandHandler createHandler,
    IUpdateTransactionCommandHandler updateHandler,
    IDeleteTransactionCommandHandler deleteHandler
) : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(
        CancellationToken ct
    )
    {
        var result = await getHandler.HandleAsync(new GetTransactionsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TransactionDto>> GetTransactionById(
        Guid id,
        CancellationToken ct
    )
    {
        var transaction = await getByIdHandler.HandleAsync(new GetTransactionByIdQuery(id), ct);
        return Ok(transaction);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTransaction(
        CreateTransactionCommand command,
        CancellationToken ct
    )
    {
        var id = await createHandler.HandleAsync(command, ct);
        return CreatedAtAction(nameof(GetTransactionById), new { id }, id);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateTransaction(
        Guid id,
        UpdateTransactionCommand command,
        CancellationToken ct
    )
    {
        if (EnsureMatchingId(id, command.Id) is ActionResult error)
            return error;

        await updateHandler.HandleAsync(command, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> DeleteTransaction(Guid id, CancellationToken ct)
    {
        await deleteHandler.HandleAsync(new DeleteTransactionCommand(id), ct);
        return NoContent();
    }
}
