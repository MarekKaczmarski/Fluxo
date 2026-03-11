namespace Fluxo.Application.Accounts.Commands.CreateAccount;

public record CreateAccountCommand(string Name, string? Description, decimal InitialBalance, string Currency = "PLN");
