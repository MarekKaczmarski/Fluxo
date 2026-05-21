using Fluxo.Domain.Exceptions;

namespace Fluxo.Domain.ValueObjects;

public sealed record Money
{
    public decimal Amount { get; private set; }
    public Currency Currency { get; private set; } = default!;

    private Money() { }

    public Money(decimal amount, Currency currency)
    {
        Currency = currency ?? throw new DomainException("Currency is required.");
        Amount = amount;
    }

    public static Money Zero(Currency currency) => new(0, currency);

    public static Money Positive(decimal amount, Currency currency)
    {
        return new Money(amount, currency).EnsurePositive();
    }

    public Money Add(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        EnsureSameCurrency(other);
        return new Money(Amount - other.Amount, Currency);
    }

    public Money EnsurePositive()
    {
        if (Amount <= 0)
            throw new DomainException("Money amount must be greater than zero.");

        return this;
    }

    private void EnsureSameCurrency(Money other)
    {
        if (Currency != other.Currency)
            throw new DomainException("Cannot operate on money with different currencies.");
    }
}
