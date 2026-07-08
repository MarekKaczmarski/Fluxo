using System.Linq.Expressions;
using Fluxo.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fluxo.Infrastructure.Data.Configurations;

public static class MoneyConfigurationExtensions
{
    public static void OwnsMoney<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, Money>> navigation,
        string amountColumnName
    )
        where TEntity : class
    {
        builder.OwnsOne(
            navigation,
            money =>
            {
                money
                    .Property(m => m.Amount)
                    .HasColumnName(amountColumnName)
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

        builder.Navigation(navigation).IsRequired();
    }
}
