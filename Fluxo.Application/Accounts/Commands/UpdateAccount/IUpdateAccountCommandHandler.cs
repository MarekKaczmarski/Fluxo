namespace Fluxo.Application.Accounts.Commands.UpdateAccount;

public interface IUpdateAccountCommandHandler
{
    Task HandleAsync(UpdateAccountCommand command, CancellationToken ct = default);
}
