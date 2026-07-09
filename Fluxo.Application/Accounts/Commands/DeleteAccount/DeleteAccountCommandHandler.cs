using Fluxo.Application.Common;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Fluxo.Application.Accounts.Commands.DeleteAccount;

public class DeleteAccountCommandHandler(IFluxoDbContext context) : IDeleteAccountCommandHandler
{
    public async Task HandleAsync(DeleteAccountCommand command, CancellationToken ct)
    {
        try
        {
            var account = await context
                .Accounts.Where(a => a.Id == command.Id)
                .ExecuteDeleteAsync(ct);

            if (account == 0)
            {
                throw new NotFoundException($"Account with ID {command.Id} was not found.");
            }
        }
        catch (PostgresException ex) when (PostgresConstraintHelper.IsForeignKeyViolation(ex))
        {
            throw new ConflictException("Account cannot be deleted because it has transactions.");
        }
    }
}
