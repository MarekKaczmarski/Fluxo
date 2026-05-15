namespace Fluxo.Application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(string Name, decimal InitialBalance, string Currency = "PLN");
