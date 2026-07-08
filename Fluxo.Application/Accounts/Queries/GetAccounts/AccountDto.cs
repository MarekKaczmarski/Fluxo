namespace Fluxo.Application.Accounts.Queries.GetAccounts;

public record AccountDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Balance { get; init; }
    public string Currency { get; init; } = "PLN";
}
