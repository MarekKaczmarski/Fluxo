using Fluxo.Application.Accounts.Commands.CreateAccount;
using Fluxo.Application.Accounts.Commands.DeleteAccount;
using Fluxo.Application.Accounts.Commands.UpdateAccount;
using Fluxo.Application.Accounts.Queries.GetAccounts;
using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountsController(
        IGetAccountsQueryHandler getAccountsHandler,
        ICreateAccountCommandHandler createAccountHandler,
        IUpdateAccountCommandHandler updateAccountHandler,
        IDeleteAccountCommandHandler deleteAccountHandler
    ) : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts(CancellationToken ct)
        {
            var accounts = await getAccountsHandler.HandleAsync(new GetAccountsQuery(), ct);
            return Ok(accounts);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAccount(
            CreateAccountCommand command,
            CancellationToken ct
        )
        {
            var accountId = await createAccountHandler.HandleAsync(command, ct);
            return CreatedAtAction(nameof(GetAccounts), new { id = accountId }, accountId);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult> UpdateAccount(
            Guid id,
            UpdateAccountCommand command,
            CancellationToken ct
        )
        {
            if (EnsureMatchingId(id, command.Id) is { } error)
                return error;

            await updateAccountHandler.HandleAsync(command, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteAccount(Guid id, CancellationToken ct)
        {
            await deleteAccountHandler.HandleAsync(new DeleteAccountCommand(id), ct);
            return NoContent();
        }
    }
}
