using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public class GetTransactionsQueryHandler(IFluxoDbContext context) : IGetTransactionsHandler
{
    public async Task<IEnumerable<TransactionDto>> HandleAsync(GetTransactionsQuery query, CancellationToken ct)
    {
        return await context.Transactions
            .AsNoTracking()
            .Include(t => t.Category)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Description = t.Description,
                Amount = t.Amount,
                Date = t.Date,
                CategoryId = t.CategoryId,
                CategoryName = t.Category.Name
            })
            .ToListAsync(ct);
    }
}