using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Fluxo.Application.Transactions.Commands.DeleteTransaction;
using Fluxo.Application.Transactions.Queries.GetTransactions;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(
    IGetTransactionsHandler getHandler,
    ICreateTransactionHandler createHandler,
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
    public async Task<ActionResult<Guid>> Create(CreateTransactionCommand command)
    {
        var id = await createHandler.Handle(command, default);
        return Ok(id);
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(Guid id, CancellationToken ct)
    {
        await deleteHandler.Handle(new DeleteTransactionCommand(id), default);
        return NoContent();
    }
}