using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Domain.Entities
{
    public class Account
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "PLN";

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
