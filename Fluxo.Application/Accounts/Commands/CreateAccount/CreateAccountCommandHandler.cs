using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler(
        IFluxoDbContext context,
        IValidator<CreateAccountCommand> validator) : ICreateAccountCommandHandler
    {
        public async Task<Guid> HandleAsync(CreateAccountCommand command, CancellationToken ct)
        {
            var validationResult = await validator.ValidateAsync(command, ct);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var account = new Account
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Balance = command.InitialBalance,
                Currency = command.Currency
            };

            context.Accounts.Add(account);
            await context.SaveChangesAsync(ct);

            return account.Id;
        }
    }
}
