using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Fluxo.Application.Transactions.Commands.DeleteTransaction;
using Fluxo.Application.Transactions.Commands.UpdateTransaction;
using Fluxo.Application.Transactions.Queries.GetTransactions;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(
    IGetTransactionsHandler getHandler,
    ICreateTransactionHandler createHandler,
    IUpdateTransactionHandler updateHandler,
    IDeleteTransactionHandler deleteHandler) 
    : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TransactionDto>>> GetTransactions(CancellationToken ct)
    {
        var result = await getHandler.HandleAsync(new GetTransactionsQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateTransaction(CreateTransactionCommand command, CancellationToken ct)
    {
        var id = await createHandler.HandleAsync(command, default);
        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> UpdateTransaction(Guid id, UpdateTransactionCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in URL does not match ID in body.");
        }

        await updateHandler.HandleAsync(command, ct);

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteTransaction(Guid id, CancellationToken ct)
    {
        await deleteHandler.HandleAsync(new DeleteTransactionCommand(id), default);
        return NoContent();
    }
}