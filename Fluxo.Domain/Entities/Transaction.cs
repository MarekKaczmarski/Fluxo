using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Fluxo.Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; } = default!;
        public Guid AccountId { get; set; }
    }
}
