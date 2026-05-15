using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Commands.UpdateAccount;

public class UpdateAccountCommandHandler(
    IFluxoDbContext context,
    IValidator<UpdateAccountCommand> validator) : IUpdateAccountCommandHandler
{
    public async Task HandleAsync(UpdateAccountCommand command, CancellationToken ct = default)
    {
        var validationResult = await validator.ValidateAsync(command, ct);
        if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

        var account = await context.Accounts
            .FirstOrDefaultAsync(a => a.Id == command.Id, ct);

        if (account is null)
            throw new NotFoundException($"Account with ID {command.Id} was not found.");

        account.UpdateName(command.Name);
        await context.SaveChangesAsync(ct);
    }
}
