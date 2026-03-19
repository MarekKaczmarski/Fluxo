using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;

namespace Fluxo.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
    IFluxoDbContext context,
    IValidator<CreateCategoryCommand> validator) : ICreateCategoryCommandHandler
{
    public async Task<Guid> HandleAsync(CreateCategoryCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Icon = command.Icon
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync(ct);

        return category.Id;
    }
}