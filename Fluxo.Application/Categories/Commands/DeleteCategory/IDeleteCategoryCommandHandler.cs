namespace Fluxo.Application.Categories.Commands.DeleteCategory;

public interface IDeleteCategoryCommandHandler
{
    Task HandleAsync(DeleteCategoryCommand command, CancellationToken ct);
}
