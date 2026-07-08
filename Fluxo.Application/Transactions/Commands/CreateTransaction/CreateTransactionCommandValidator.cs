using FluentValidation;
using Fluxo.Application.Common.Interfaces;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator(
        IAccountExistenceChecker accountExistenceChecker,
        ICategoryExistenceChecker categoryExistenceChecker
    )
    {
        RuleFor(v => v.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(200)
            .WithMessage("Description must not exceed 200 characters.");

        RuleFor(v => v.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(v => v.Date)
            .NotEmpty()
            .WithMessage("Date is required.")
            .Must(date => date.Date <= DateTime.UtcNow.Date)
            .WithMessage("Date cannot be in the future.");

        RuleFor(v => v.AccountId)
            .NotEmpty()
            .WithMessage("AccountId is required.")
            .MustAsync((id, ct) => accountExistenceChecker.ExistsAsync(id, ct))
            .WithMessage("Account does not exist.");

        RuleFor(v => v.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required.")
            .MustAsync((id, ct) => categoryExistenceChecker.ExistsAsync(id, ct))
            .WithMessage("Category does not exist.");

        RuleFor(v => v.Type).IsInEnum().WithMessage("Transaction type is invalid.");
    }
}
