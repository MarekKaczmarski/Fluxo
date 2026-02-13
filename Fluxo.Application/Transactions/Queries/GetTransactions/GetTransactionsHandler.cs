using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Transactions.Queries.GetTransactions;
using Microsoft.EntityFrameworkCore;

public class GetTransactionsHandler(IFluxoDbContext context) : IGetTransactionsHandler
{
    public async Task<List<TransactionDto>> Handle(GetTransactionsQuery query, CancellationToken ct)
    {
        return await context.Transactions
            .AsNoTracking()
            .OrderByDescending(x => x.Date)
            .Select(t => new TransactionDto(
                t.Id,
                t.Description,
                t.Amount,
                t.Date,
                t.CategoryId,
                t.AccountId))
            .ToListAsync(ct);
    }
}