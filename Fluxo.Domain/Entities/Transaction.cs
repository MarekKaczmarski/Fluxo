using Fluxo.Domain.Enums;
using Fluxo.Domain.Exceptions;
using Fluxo.Domain.ValueObjects;

namespace Fluxo.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; private set; }
        public string Description { get; private set; } = string.Empty;
        public Money Amount { get; private set; } = default!;
        public DateTime Date { get; private set; }
        public Guid CategoryId { get; private set; }
        public Category Category { get; private set; } = default!;
        public Guid AccountId { get; private set; }
        public Account Account { get; private set; } = default!;
        public TransactionType Type { get; private set; }

        private Transaction() { }

        public Transaction(Guid id, Money amount, string description, DateTime date, Guid categoryId, Guid accountId, TransactionType type)
        {
            Validate(id, amount, description, date, categoryId, accountId, type);

            Id = id;
            Amount = amount.EnsurePositive();
            Description = description.Trim();
            Date = date;
            CategoryId = categoryId;
            AccountId = accountId;
            Type = type;
        }

        public void Update(Money amount, string description, DateTime date, Guid categoryId, TransactionType type)
        {
            Validate(Id, amount, description, date, categoryId, AccountId, type);

            Amount = amount.EnsurePositive();
            Description = description.Trim();
            Date = date;
            CategoryId = categoryId;
            Type = type;
        }

        private static void Validate(Guid id, Money amount, string description, DateTime date, Guid categoryId, Guid accountId, TransactionType type)
        {
            if (id == Guid.Empty)
                throw new DomainException("Transaction ID is required.");

            amount.EnsurePositive();

            if (string.IsNullOrWhiteSpace(description))
                throw new DomainException("Transaction description is required.");

            if (description.Length > 200)
                throw new DomainException("Transaction description must not exceed 200 characters.");

            if (date == default)
                throw new DomainException("Transaction date is required.");

            if (categoryId == Guid.Empty)
                throw new DomainException("Category ID is required.");

            if (accountId == Guid.Empty)
                throw new DomainException("Account ID is required.");

            if (!Enum.IsDefined(type))
                throw new DomainException("Transaction type is invalid.");
        }
    }
}
