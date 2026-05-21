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

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        private Account() { }

        public Account(Guid id, string name, decimal balance, string currency)
        {
            Validate(id, name);

            var accountCurrency = Currency.FromCode(currency);

            Id = id;
            Name = name.Trim();
            Balance = new Money(balance, accountCurrency);
        }

        public Transaction RegisterTransaction(Guid transactionId, decimal amount, string description, DateTime date, Guid categoryId, TransactionType type)
        {
            var money = Money.Positive(amount, Currency);
            var transaction = new Transaction(transactionId, money, description, date, categoryId, Id, type);

            ApplyTransaction(transaction.Amount, transaction.Type);
            Transactions.Add(transaction);

            return transaction;
        }

        public void UpdateTransaction(Transaction transaction, string description, decimal amount, DateTime date, Guid categoryId, TransactionType type)
        {
            EnsureTransactionBelongsToAccount(transaction);

            RevertTransaction(transaction.Amount, transaction.Type);
            transaction.Update(Money.Positive(amount, Currency), description, date, categoryId, type);
            ApplyTransaction(transaction.Amount, transaction.Type);
        }

        public void RemoveTransaction(Transaction transaction)
        {
            EnsureTransactionBelongsToAccount(transaction);

            RevertTransaction(transaction.Amount, transaction.Type);
            Transactions.Remove(transaction);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Account name can't be empty.");

            if (name.Length > 100)
                throw new DomainException("Account name must not exceed 100 characters.");

            Name = name.Trim();
        }

        private void ApplyTransaction(Money amount, TransactionType type)
        {
            Balance = type switch
            {
                TransactionType.Expense => Balance.Subtract(amount),
                TransactionType.Income => Balance.Add(amount),
                _ => throw new DomainException("Transaction type is invalid.")
            };
        }

        private void RevertTransaction(Money amount, TransactionType type)
        {
            Balance = type switch
            {
                TransactionType.Expense => Balance.Add(amount),
                TransactionType.Income => Balance.Subtract(amount),
                _ => throw new DomainException("Transaction type is invalid.")
            };
        }

        private void EnsureTransactionBelongsToAccount(Transaction transaction)
        {
            if (transaction.AccountId != Id)
                throw new DomainException("Transaction does not belong to this account.");
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
