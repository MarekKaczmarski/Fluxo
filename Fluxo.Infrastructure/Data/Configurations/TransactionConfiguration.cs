using Fluxo.Domain.Entities;
using Fluxo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluxo.Infrastructure.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Description).IsRequired().HasMaxLength(200);

        builder.OwnsOne(
            t => t.Amount,
            money =>
            {
                money
                    .Property(m => m.Amount)
                    .HasColumnName("Amount")
                    .HasPrecision(18, 2)
                    .IsRequired();

                money
                    .Property(m => m.Currency)
                    .HasConversion(c => c.Code, code => Currency.FromCode(code))
                    .HasColumnName("Currency")
                    .HasMaxLength(3)
                    .IsRequired();
            }
        );

        builder.Navigation(t => t.Amount).IsRequired();

        builder.Property(t => t.Date).IsRequired();

        builder.HasIndex(t => t.Date);

        builder
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(t => t.AccountId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(t => t.Category)
            .WithMany()
            .HasForeignKey(t => t.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        //builder.HasData(TransactionSeeder.GetSeedData().ToArray());
    }
}
