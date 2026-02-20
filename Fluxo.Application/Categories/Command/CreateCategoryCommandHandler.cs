using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;
using Fluxo.Infrastructure.Data;

namespace Fluxo.Application.Categories.Commands.CreateCategory;

public class CreateCategoryHandler(FluxoDbContext context) : ICreateCategoryHandler
{
    public async Task<Guid> HandleAsync(CreateCategoryCommand command, CancellationToken ct)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Icon = command.Icon,
            Color = command.Color
        };

        context.Categories.Add(category);
        await context.SaveChangesAsync(ct);

        return category.Id;
    }
}