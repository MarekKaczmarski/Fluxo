using FluentValidation;
using Fluxo.Application.Common.Interfaces;

namespace Fluxo.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator(ICategoryUniquenessChecker uniquenessChecker)
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Category ID is required.");

        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(50)
            .WithMessage("Category name cannot exceed 50 characters.")
            .MustAsync(
                (command, name, ct) => BeUniqueNameAsync(uniquenessChecker, command.Id, name, ct)
            )
            .WithMessage("Category with this name already exists.");

        RuleFor(x => x.Icon).MaximumLength(50).WithMessage("Icon name is too long.");
    }

    private static async Task<bool> BeUniqueNameAsync(
        ICategoryUniquenessChecker checker,
        Guid currentId,
        string name,
        CancellationToken ct
    )
    {
        return !await checker.IsNameTakenAsync(name, excludeId: currentId, ct);
    }
}
