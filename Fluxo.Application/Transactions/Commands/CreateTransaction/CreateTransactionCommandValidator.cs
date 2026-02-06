using FluentValidation;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(v => v.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(50).WithMessage("Category must not exceed 50 characters.");

        RuleFor(v => v.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");

        RuleFor(v => v.AccountId)
            .NotEmpty().WithMessage("AccountId is required.");
    }
}