using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Fluxo.Application.Transactions.Queries.GetTransactions;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(
    IGetTransactionsHandler getHandler,
    ICreateTransactionHandler handler) : ControllerBase
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
        var id = await handler.Handle(command, default);
        return Ok(id);
    }
}