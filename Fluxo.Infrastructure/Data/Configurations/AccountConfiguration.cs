using Fluxo.Domain.Entities;
using Fluxo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluxo.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name).IsRequired().HasMaxLength(100);

        builder.HasIndex(a => a.Name).IsUnique();

        builder.Ignore(a => a.Currency);

        builder.OwnsOne(
            a => a.Balance,
            money =>
            {
                money
                    .Property(m => m.Amount)
                    .HasColumnName("Balance")
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

        builder.Navigation(a => a.Balance).IsRequired();

        builder.Property(a => a.Version).HasColumnName("xmin").HasColumnType("xid").IsRowVersion();

        //builder.HasData(AccountSeeder.GetSeedData());
    }
}
