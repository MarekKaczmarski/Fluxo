using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    private readonly IFluxoDbContext _context;

    public UpdateAccountCommandValidator(IFluxoDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Account ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Account name is required.")
            .MaximumLength(100).WithMessage("Account name is too long.")
            .MustAsync(BeUniqueName).WithMessage("Account with this name already exists.");
    }

    private async Task<bool> BeUniqueName(UpdateAccountCommand command, string name, CancellationToken ct)
    {
        return !await _context.Accounts
            .AnyAsync(a => a.Id != command.Id && a.Name.ToLower() == name.ToLower(), ct);
    }
}
