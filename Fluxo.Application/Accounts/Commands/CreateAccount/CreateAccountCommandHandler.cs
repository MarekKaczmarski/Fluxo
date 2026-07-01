using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler(
        IFluxoDbContext context,
        IValidator<CreateAccountCommand> validator
    ) : ICreateAccountCommandHandler
    {
        public async Task<Guid> HandleAsync(CreateAccountCommand command, CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var account = new Account(
                Guid.NewGuid(),
                command.Name,
                command.InitialBalance,
                command.Currency
            );

            context.Accounts.Add(account);

            try
            {
                await context.SaveChangesAsync(ct);
            }
            catch (DbUpdateException ex) when (IsUniqueConstraintViolation(ex))
            {
                throw new ConflictException($"Account with name '{command.Name}' already exists.");
            }

            return account.Id;
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex) =>
            ex.InnerException?.Message.Contains(
                "IX_Accounts_Name",
                StringComparison.OrdinalIgnoreCase
            ) ?? false;
    }
}
