using FluentValidation;
using Fluxo.Application.Common.Interfaces;

namespace Fluxo.Application.Accounts.Commands.CreateAccount;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator(IAccountUniquenessChecker uniquenessChecker)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Account name is required.")
            .MaximumLength(100)
            .WithMessage("Account name is too long.")
            .MustAsync((name, ct) => BeUniqueNameAsync(uniquenessChecker, name, ct))
            .WithMessage("Account with this name already exists.");

        RuleFor(x => x.Currency)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Currency is required.")
            .Length(3)
            .WithMessage("Currency must be a 3-letter ISO code.")
            .Matches("^[A-Za-z]{3}$")
            .WithMessage("Currency must contain only letters.");
    }

    private static async Task<bool> BeUniqueNameAsync(
        IAccountUniquenessChecker checker,
        string name,
        CancellationToken ct
    )
    {
        return !await checker.IsNameTakenAsync(name, excludeId: null, ct);
    }
}
