using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler(IFluxoDbContext context) : IDeleteCategoryCommandHandler
    {
        public async Task HandleAsync(DeleteCategoryCommand command, CancellationToken ct)
        {
            var category = await context.Categories.FindAsync([command.Id], ct);

            if (category is null)
                throw new NotFoundException($"Category with ID {command.Id} was not found.");

            var hasTransactions = await context.Transactions
                .AnyAsync(t => t.CategoryId == command.Id, ct);

            if (hasTransactions)
                throw new ConflictException("Category cannot be deleted because it has transactions.");

            context.Categories.Remove(category);
            await context.SaveChangesAsync(ct);
        }
    }
}
