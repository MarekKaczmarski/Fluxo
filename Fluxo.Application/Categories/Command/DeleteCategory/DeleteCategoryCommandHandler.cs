
using Fluxo.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Application.Categories.Command.DeleteCategory
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
