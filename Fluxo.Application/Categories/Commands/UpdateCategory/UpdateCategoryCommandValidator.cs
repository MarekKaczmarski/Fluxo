using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    private readonly IFluxoDbContext _context;

    public UpdateCategoryCommandValidator(IFluxoDbContext context)
    {
        _context = context;

        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Category ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters.")
            .MustAsync(BeUniqueName).WithMessage("Category with this name already exists.");

        RuleFor(x => x.Icon)
            .MaximumLength(50).WithMessage("Icon name is too long.");
    }

    private async Task<bool> BeUniqueName(UpdateCategoryCommand command, string name, CancellationToken ct)
    {
        return !await _context.Categories
            .AnyAsync(c => c.Id != command.Id && c.Name.ToLower() == name.ToLower(), ct);
    }
}
