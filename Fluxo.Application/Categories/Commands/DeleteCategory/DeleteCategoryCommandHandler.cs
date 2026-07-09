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
        try
        {
            var category = await context
                .Categories.Where(c => c.Id == command.Id)
                .ExecuteDeleteAsync(ct);

            if (category == 0)
            {
                throw new NotFoundException($"Category with ID {command.Id} was not found.");
            }
        }
        catch (PostgresException ex) when (PostgresConstraintHelper.IsForeignKeyViolation(ex))
        {
            throw new ConflictException("Category cannot be deleted because it has transactions.");
        }
    }
}
