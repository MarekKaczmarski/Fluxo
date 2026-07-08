using FluentValidation;
using Fluxo.Application.Common.Interfaces;

namespace Fluxo.Application.Transactions.Commands.UpdateTransaction;

public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
{
    public UpdateTransactionCommandValidator(
        IAccountExistenceChecker accountExistenceChecker,
        ICategoryExistenceChecker categoryExistenceChecker
    )
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Transaction ID is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(200)
            .WithMessage("Description must not exceed 200 characters.");

        RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required.")
            .Must(date => date.Date <= DateTime.UtcNow.Date)
            .WithMessage("Date cannot be in the future.");

        RuleFor(x => x.AccountId)
            .NotEmpty()
            .WithMessage("AccountId is required.")
            .MustAsync((id, ct) => accountExistenceChecker.ExistsAsync(id, ct))
            .WithMessage("Account does not exist.");

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required.")
            .MustAsync((id, ct) => categoryExistenceChecker.ExistsAsync(id, ct))
            .WithMessage("Category does not exist.");

        RuleFor(x => x.Type).IsInEnum().WithMessage("Transaction type is invalid.");
    }
}
