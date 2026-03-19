using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandHandler(IFluxoDbContext context) : IUpdateCategoryCommandHandler
{
    public async Task HandleAsync(UpdateCategoryCommand command, CancellationToken ct)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.Id == command.Id, ct);

        if (category == null) return;

        category.Name = command.Name;
        category.Icon = command.Icon;

        await context.SaveChangesAsync(ct);
    }
}