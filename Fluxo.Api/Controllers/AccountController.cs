using Fluxo.Application.Accounts.Commands.CreateAccount;
using Fluxo.Application.Accounts.Commands.DeleteAccount;
using Fluxo.Application.Accounts.Commands.UpdateAccount;
using Fluxo.Application.Accounts.Queries.GetAccounts;
using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IGetAccountsQueryHandler _getAccountsHandler;
        private readonly ICreateAccountCommandHandler _createAccountHandler;
        private readonly IUpdateAccountCommandHandler _updateAccountHandler;
        private readonly IDeleteAccountCommandHandler _deleteAccountHandler;

        public AccountsController(
            IGetAccountsQueryHandler getAccountsHandler,
            ICreateAccountCommandHandler createAccountHandler,
            IUpdateAccountCommandHandler updateAccountHandler,
            IDeleteAccountCommandHandler deleteAccountHandler)
        {
            _getAccountsHandler = getAccountsHandler;
            _createAccountHandler = createAccountHandler;
            _updateAccountHandler = updateAccountHandler;
            _deleteAccountHandler = deleteAccountHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAccounts(CancellationToken ct)
        {
            var accounts = await _getAccountsHandler.HandleAsync();
            return Ok(accounts);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateAccount(CreateAccountCommand command, CancellationToken ct)
        {
            var accountId = await _createAccountHandler.HandleAsync(command);
            return Ok(accountId);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAccount(Guid id, UpdateAccountCommand command, CancellationToken ct)
        {
            if (id != command.Id) return BadRequest();

            await _updateAccountHandler.HandleAsync(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAccount(Guid id, CancellationToken ct)
        {
            await _deleteAccountHandler.HandleAsync(id);
            return NoContent();
        }
    }
}
