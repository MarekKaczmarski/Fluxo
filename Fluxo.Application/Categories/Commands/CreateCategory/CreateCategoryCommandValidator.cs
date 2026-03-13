using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IFluxoDbContext _context;

        public CreateCategoryCommandValidator(IFluxoDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(50).WithMessage("Category name cannot exceed 50 characters.")
                .MustAsync(BeUniqueName).WithMessage("Category with this name already exists.");

            RuleFor(x => x.Icon)
                .MaximumLength(50).WithMessage("Icon name is too long.");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken ct)
        {
            return !await _context.Categories
                .AnyAsync(c => c.Name.ToLower() == name.ToLower(), ct);
        }
    }
}
