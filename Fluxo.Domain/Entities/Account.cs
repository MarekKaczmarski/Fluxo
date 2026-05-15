using Fluxo.Domain.Enums;
using Fluxo.Domain.Exceptions;

namespace Fluxo.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = default!;
        public decimal Balance { get; private set; }
        public string Currency { get; private set; } = "PLN";

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        private Account() { }

        public Account(Guid id, string name, decimal balance, string currency)
        {
            Validate(id, name, currency);

            Id = id;
            Name = name.Trim();
            Balance = balance;
            Currency = currency.Trim().ToUpperInvariant();
        }

        public Transaction RegisterTransaction(Guid transactionId, decimal amount, string description, DateTime date, Guid categoryId, TransactionType type)
        {
            var transaction = new Transaction(transactionId, amount, description, date, categoryId, Id, type);
            Balance += GetBalanceDelta(transaction.Amount, transaction.Type);
            Transactions.Add(transaction);

            return transaction;
        }

        public void UpdateTransaction(Transaction transaction, string description, decimal amount, DateTime date, Guid categoryId, TransactionType type)
        {
            EnsureTransactionBelongsToAccount(transaction);

            Balance -= GetBalanceDelta(transaction.Amount, transaction.Type);
            transaction.Update(description, amount, date, categoryId, type);
            Balance += GetBalanceDelta(transaction.Amount, transaction.Type);
        }

        public void RemoveTransaction(Transaction transaction)
        {
            EnsureTransactionBelongsToAccount(transaction);

            Balance -= GetBalanceDelta(transaction.Amount, transaction.Type);
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

        private static decimal GetBalanceDelta(decimal amount, TransactionType type)
        {
            if (amount <= 0)
                throw new DomainException("Transaction amount must be greater than zero.");

            return type switch
            {
                TransactionType.Expense => -amount,
                TransactionType.Income => amount,
                _ => throw new DomainException("Transaction type is invalid.")
            };
        }

        private void EnsureTransactionBelongsToAccount(Transaction transaction)
        {
            if (transaction.AccountId != Id)
                throw new DomainException("Transaction does not belong to this account.");
        }

        private static void Validate(Guid id, string name, string currency)
        {
            if (id == Guid.Empty)
                throw new DomainException("Account ID is required.");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Account name can't be empty.");

            if (name.Length > 100)
                throw new DomainException("Account name must not exceed 100 characters.");

            if (string.IsNullOrWhiteSpace(currency))
                throw new DomainException("Currency is required.");

            if (currency.Trim().Length != 3)
                throw new DomainException("Currency must be a 3-letter ISO code.");
        }
    }
}
