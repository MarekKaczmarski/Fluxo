namespace Fluxo.Application.Categories.Commands.UpdateCategory
{
    public interface IUpdateCategoryHandler
    {
        Task HandleAsync(UpdateCategoryCommand command, CancellationToken ct);
    }
}
