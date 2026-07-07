using FluentValidation;
using Fluxo.Application.Common.Interfaces;

namespace Fluxo.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator(ICategoryUniquenessChecker uniquenessChecker)
    {
        RuleFor(x => x.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(50)
            .WithMessage("Category name cannot exceed 50 characters.")
            .MustAsync((name, ct) => BeUniqueNameAsync(uniquenessChecker, name, ct))
            .WithMessage("Category with this name already exists.");

        RuleFor(x => x.Icon).MaximumLength(50).WithMessage("Icon name is too long.");
    }

    private static async Task<bool> BeUniqueNameAsync(
        ICategoryUniquenessChecker checker,
        string name,
        CancellationToken ct
    )
    {
        return !await checker.IsNameTakenAsync(name, excludeId: null, ct);
    }
}
