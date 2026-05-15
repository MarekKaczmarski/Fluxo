using FluentValidation;

namespace Fluxo.Application.Transactions.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(v => v.Description)
            .NotEmpty().WithMessage("Description is required.")
            .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

        RuleFor(v => v.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");

        RuleFor(v => v.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");

        RuleFor(v => v.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.AccountId)
            .NotEmpty().WithMessage("AccountId is required.");

        RuleFor(v => v.Type)
            .IsInEnum().WithMessage("Transaction type is invalid.");
    }
}
