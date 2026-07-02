using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(
    IFluxoDbContext context,
    IValidator<UpdateCategoryCommand> validator
) : IUpdateCategoryCommandHandler
{
    public async Task HandleAsync(UpdateCategoryCommand command, CancellationToken ct)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var category =
            await context.Categories.FirstOrDefaultAsync(c => c.Id == command.Id, ct)
            ?? throw new NotFoundException($"Category with ID {command.Id} was not found.");

        category.Update(command.Name, command.Icon);

        await context.SaveChangesAsync(ct);
    }
}
