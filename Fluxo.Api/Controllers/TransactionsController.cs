using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ICreateTransactionHandler _handler;

    public TransactionsController(ICreateTransactionHandler handler)
    {
        _handler = handler;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateTransactionCommand command)
    {
        var result = await _handler.Handle(command, default);
        return Ok(result);
    }
}