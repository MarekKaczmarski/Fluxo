using Fluxo.Domain.Enums;
using Fluxo.Domain.Exceptions;
using Fluxo.Domain.ValueObjects;

namespace Fluxo.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public Money Balance { get; private set; } = default!;
        public Currency Currency => Balance.Currency;

        private Account() { }

        public Account(Guid id, string name, decimal balance, string currency)
        {
            Validate(id, name);

            var accountCurrency = Currency.FromCode(currency);

            Id = id;
            Name = name.Trim();
            Balance = new Money(balance, accountCurrency);
        }

        public void ApplyTransaction(Money amount, TransactionType type)
        {
            Balance = type switch
            {
                TransactionType.Expense => Balance.Subtract(amount),
                TransactionType.Income => Balance.Add(amount),
                _ => throw new DomainException("Transaction type is invalid.")
            };
        }

        public void RevertTransaction(Money amount, TransactionType type)
        {
            Balance = type switch
            {
                TransactionType.Expense => Balance.Add(amount),
                TransactionType.Income => Balance.Subtract(amount),
                _ => throw new DomainException("Transaction type is invalid.")
            };
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Account name can't be empty.");

            if (name.Length > 100)
                throw new DomainException("Account name must not exceed 100 characters.");

            Name = name.Trim();
        }

        private static void Validate(Guid id, string name)
        {
            if (id == Guid.Empty)
                throw new DomainException("Account ID is required.");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Account name can't be empty.");

            if (name.Length > 100)
                throw new DomainException("Account name must not exceed 100 characters.");
        }
    }
}