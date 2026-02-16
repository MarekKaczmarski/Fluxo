using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Data;

public class FluxoDbContext : DbContext, IFluxoDbContext
{
    public FluxoDbContext(DbContextOptions<FluxoDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FluxoDbContext).Assembly);
    }
}