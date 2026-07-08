using Fluxo.Domain.Entities;
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

        builder.OwnsMoney(a => a.Balance, "Balance");

        builder.Property(a => a.Version).HasColumnName("xmin").HasColumnType("xid").IsRowVersion();

        //builder.HasData(AccountSeeder.GetSeedData());
    }
}
