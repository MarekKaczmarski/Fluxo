using Fluxo.Application.Common.Interfaces;
using Fluxo.Application.Exceptions;
using Fluxo.Application.Transactions.Queries.GetTransactions;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Queries.GetTransactionById;

public class GetTransactionByIdQueryHandler(IFluxoDbContext context)
    : IGetTransactionByIdQueryHandler
{
    public async Task<TransactionDto> HandleAsync(
        GetTransactionByIdQuery query,
        CancellationToken ct
    )
    {
        return await context
                .Transactions.AsNoTracking()
                .Where(t => t.Id == query.Id)
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
                .FirstOrDefaultAsync(ct)
            ?? throw new NotFoundException($"Transaction with ID {query.Id} was not found.");
    }
}
