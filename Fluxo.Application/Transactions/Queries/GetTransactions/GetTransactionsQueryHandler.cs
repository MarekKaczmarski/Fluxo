using FluentValidation;
using Fluxo.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Transactions.Queries.GetTransactions;

public class GetTransactionsQueryHandler(
    IFluxoDbContext context,
    IValidator<GetTransactionsQuery> validator
) : IGetTransactionsQueryHandler
{
    public async Task<List<TransactionDto>> HandleAsync(
        GetTransactionsQuery query,
        CancellationToken ct
    )
    {
        var validationResult = await validator.ValidateAsync(query, ct);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        return await context
            .Transactions.AsNoTracking()
            .OrderByDescending(t => t.Date)
            .Skip((query.PageNumber - 1) * query.PageSize)
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
