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
    public async Task<ActionResult<List<TransactionDto>>> Get(CancellationToken ct)
    {
        var result = await getHandler.Handle(new GetTransactionsQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateTransactionCommand command, CancellationToken ct)
    {
        var id = await createHandler.Handle(command, default);
        return Ok(id);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateTransactionCommand command, CancellationToken ct)
    {
        if (id != command.Id)
        {
            return BadRequest("ID in URL does not match ID in body.");
        }

        await updateHandler.Handle(command, ct);

        return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
    {
        await deleteHandler.Handle(new DeleteTransactionCommand(id), default);
        return NoContent();
    }
}