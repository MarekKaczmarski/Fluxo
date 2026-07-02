using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public class GetTransactionsQueryHandler(IFluxoDbContext context) : IGetTransactionsQueryHandler
{
    public Task<List<TransactionDto>> HandleAsync(GetTransactionsQuery query, CancellationToken ct)
    {
        return context
            .Transactions.AsNoTracking()
            .Include(t => t.Category)
            .OrderByDescending(t => t.Date)
            .Skip((Math.Max(query.PageNumber, 1) - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount.Amount,
                Date = t.Date,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name,
                Currency = t.Amount.Currency.Code,
                Type = t.Type,
            })
            .ToListAsync(ct);
    }
}
