using Fluxo.Application.Categories.Commands.UpdateCategory;

namespace Fluxo.Application.Categories.Command.UpdateCategory
{
    public interface IUpdateCategoryHandler
    {
        Task HandleAsync(UpdateCategoryCommand command, CancellationToken ct);
    }
}
