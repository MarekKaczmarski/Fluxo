using Fluxo.Domain.Enums;

namespace Fluxo.Application.Transactions.Queries.GetTransactions
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = default!;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; } = default!;
        public string Currency { get; set; } = default!;
        public TransactionType Type { get; set; }
    }
}
