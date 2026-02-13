using FluentValidation;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(100).WithMessage("Description must not exceed 100 characters.");

        RuleFor(v => v.Amount)
            .NotEqual(0).WithMessage("Amount cannot be zero.");

        RuleFor(v => v.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");

        RuleFor(v => v.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId is required.");
    }
}