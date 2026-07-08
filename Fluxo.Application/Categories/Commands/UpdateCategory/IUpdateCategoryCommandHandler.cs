namespace Fluxo.Application.Categories.Commands.UpdateCategory;

public interface IUpdateCategoryCommandHandler
{
    Task HandleAsync(UpdateCategoryCommand command, CancellationToken ct);
}
