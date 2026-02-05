using Fluxo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Application.Common.Interfaces;

public interface IFluxoDbContext
{
    DbSet<Transaction> Transactions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}