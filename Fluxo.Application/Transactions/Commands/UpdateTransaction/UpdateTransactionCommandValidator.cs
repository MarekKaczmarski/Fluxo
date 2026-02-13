using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Application.Transactions.Commands.UpdateTransaction
{
    public class UpdateTransactionCommandValidator : AbstractValidator<UpdateTransactionCommand>
    {
        public UpdateTransactionCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Transaction ID is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(200).WithMessage("Description must not exceed 200 characters.");

            RuleFor(x => x.Amount)
                .NotEqual(0).WithMessage("Amount cannot be zero.");

            RuleFor(x => x.Date)
                .NotEmpty().WithMessage("Date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("CategoryId is required.");
        }
    }
}
