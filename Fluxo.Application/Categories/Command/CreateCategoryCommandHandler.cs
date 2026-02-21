using Fluxo.Application.Categories.Command;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;

namespace Fluxo.Application.Categories.Command.CreateCategory;

public class CreateCategoryCommandHandler(IFluxoDbContext context) : ICreateCategoryHandler
{
    public async Task<Guid> HandleAsync(CreateCategoryCommand command, CancellationToken ct)
    {
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