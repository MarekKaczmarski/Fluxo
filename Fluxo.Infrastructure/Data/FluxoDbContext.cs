using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Data;

public class FluxoDbContext(DbContextOptions<FluxoDbContext> options)
    : DbContext(options),
        IFluxoDbContext
{
    public DbSet<Transaction> Transactions { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Account> Accounts { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FluxoDbContext).Assembly);
    }
}
