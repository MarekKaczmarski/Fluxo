using Fluxo.Domain.Exceptions;

namespace Fluxo.Domain.ValueObjects;

public sealed record Currency
{
    public string Code { get; private set; } = default!;

    private Currency() { }

    private Currency(string code)
    {
        Code = code;
    }

    public static Currency FromCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Currency is required.");

        var normalized = code.Trim().ToUpperInvariant();

        if (normalized.Length != 3 || !normalized.All(char.IsLetter))
            throw new DomainException("Currency must be a 3-letter ISO code.");

        return new Currency(normalized);
    }

    public override string ToString() => Code;
}
