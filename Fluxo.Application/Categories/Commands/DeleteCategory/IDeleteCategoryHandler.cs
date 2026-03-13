namespace Fluxo.Application.Categories.Commands.DeleteCategory
{
    public interface IDeleteCategoryHandler
    {
        Task HandleAsync(DeleteCategoryCommand command, CancellationToken ct);
    }
}
