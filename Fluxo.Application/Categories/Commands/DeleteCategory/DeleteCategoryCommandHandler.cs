using Fluxo.Application.Common.Interfaces;

namespace Fluxo.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryHandler(IFluxoDbContext context) : IDeleteCategoryHandler
    {
        public async Task HandleAsync(DeleteCategoryCommand command, CancellationToken ct)
        {
            var category = await context.Categories.FindAsync([command.Id], ct);
            if (category != null)
            {
                context.Categories.Remove(category);
                await context.SaveChangesAsync(ct);
            }
        }
    }
}
