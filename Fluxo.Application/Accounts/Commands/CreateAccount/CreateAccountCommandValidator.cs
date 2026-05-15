using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
    {
        private readonly IFluxoDbContext _context;

        public CreateAccountCommandValidator(IFluxoDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Account name is required.")
                .MaximumLength(100).WithMessage("Account name is too long.")
                .MustAsync(BeUniqueName).WithMessage("Account with this name already exists.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .Length(3).WithMessage("Currency must be a 3-letter ISO code.")
                .Matches("^[A-Za-z]{3}$").WithMessage("Currency must contain only letters.");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken ct)
        {
            return !await _context.Accounts
                .AnyAsync(a => a.Name.ToLower() == name.ToLower(), ct);
        }
    }
}
