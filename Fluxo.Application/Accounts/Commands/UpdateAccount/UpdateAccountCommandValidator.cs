using FluentValidation;
using Fluxo.Application.Common.Interfaces;

namespace Fluxo.Application.Accounts.Commands.UpdateAccount
{
    public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
    {
        public UpdateAccountCommandValidator(IAccountUniquenessChecker uniquenessChecker)
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Account ID is required.");

            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("Account name is required.")
                .MaximumLength(100)
                .WithMessage("Account name is too long.")
                .MustAsync(
                    (command, name, ct) =>
                        BeUniqueNameAsync(uniquenessChecker, command.Id, name, ct)
                )
                .WithMessage("Account with this name already exists.");
        }

        private static async Task<bool> BeUniqueNameAsync(
            IAccountUniquenessChecker checker,
            Guid currentId,
            string name,
            CancellationToken ct
        )
        {
            return !await checker.IsNameTakenAsync(name, excludeId: currentId, ct);
        }
    }
}
