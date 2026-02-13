using Fluxo.Application.Common.Interfaces;
using Fluxo.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fluxo.Infrastructure.Data;

public class FluxoDbContext : DbContext, IFluxoDbContext
{
    public FluxoDbContext(DbContextOptions<FluxoDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(e => e.Amount)
                .HasPrecision(18,2);

            entity.Property(e => e.Date)
                .IsRequired();

            entity.Property(e => e.CategoryId)
                .IsRequired();

            entity.Property(e => e.AccountId)
                .IsRequired();
        });
    }
}