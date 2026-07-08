using Fluxo.Application.Common;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Fluxo.Application.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(IFluxoDbContext context) : IDeleteCategoryCommandHandler
{
    public async Task HandleAsync(DeleteCategoryCommand command, CancellationToken ct)
    {
        var categoryExists = await context.Categories.AnyAsync(c => c.Id == command.Id, ct);

        if (!categoryExists)
            throw new NotFoundException($"Category with ID {command.Id} was not found.");

        var hasTransactions = await context.Transactions.AnyAsync(
            t => t.CategoryId == command.Id,
            ct
        );

        if (hasTransactions)
        {
            throw new ConflictException("Category cannot be deleted because it has transactions.");
        }

        try
        {
            await context.Categories.Where(c => c.Id == command.Id).ExecuteDeleteAsync(ct);
        }
        catch (PostgresException ex) when (PostgresConstraintHelper.IsForeignKeyViolation(ex))
        {
            throw new ConflictException("Category cannot be deleted because it has transactions.");
        }
    }
}
