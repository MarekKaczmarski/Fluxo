namespace Fluxo.Application.Accounts.Commands.CreateAccount;

public interface ICreateAccountCommandHandler
{
    Task<Guid> HandleAsync(CreateAccountCommand command, CancellationToken ct);
}
