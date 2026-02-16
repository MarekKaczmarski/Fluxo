using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fluxo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialTransactionsWithFixedGuids2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                column: "Description",
                value: "Grocery");

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: new Guid("a555e888-4444-4444-4444-111122223333"),
                columns: new[] { "Amount", "CategoryId", "Description" },
                values: new object[] { -45.00m, new Guid("d4c3b2a1-f6e5-4b5a-ac6d-9f8e7d6c5b4a"), "Pharmacy" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                column: "Description",
                value: "Grocery shopping");

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: new Guid("a555e888-4444-4444-4444-111122223333"),
                columns: new[] { "Amount", "CategoryId", "Description" },
                values: new object[] { -899.99m, new Guid("a1b2c3d4-e5f6-4a5b-bc6d-7e8f9a0b1c2d"), "Wireless Headphones" });
        }
    }
}
