using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public required string Category { get; set; }

        // Powiązanie z kontem (uproszczone na start)
        public Guid AccountId { get; set; }
    }
}
