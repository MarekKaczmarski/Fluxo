using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandHandler(
    IFluxoDbContext context,
    IValidator<CreateCategoryCommand> validator
) : ICreateCategoryCommandHandler
{
    public async Task<Guid> HandleAsync(CreateCategoryCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category = new Category(Guid.NewGuid(), command.Name, command.Icon);

        context.Categories.Add(category);

        try
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
        {
            throw new ConflictException($"Category with name '{command.Name}' already exists.");
        }

        return category.Id;
    }

    private static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
        ex.InnerException?.Message.Contains(
            "IX_Categories_Name",
            StringComparison.OrdinalIgnoreCase
        ) ?? false;
}
