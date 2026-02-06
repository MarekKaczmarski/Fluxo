using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController(ICreateTransactionHandler handler) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateTransactionCommand command)
    {
        var id = await handler.Handle(command, default);
        return Ok(id);
    }
}