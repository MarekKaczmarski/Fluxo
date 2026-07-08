namespace Fluxo.Application.Accounts.Commands.DeleteAccount;

public interface IDeleteAccountCommandHandler
{
    Task HandleAsync(DeleteAccountCommand command, CancellationToken ct);
}
