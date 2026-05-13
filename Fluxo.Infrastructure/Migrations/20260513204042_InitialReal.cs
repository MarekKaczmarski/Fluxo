using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Fluxo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialReal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Balance", "Currency", "Description", "Name" },
                values: new object[] { new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 5000.00m, "PLN", "Default personal cash wallet", "Main Wallet" });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Icon", "Name" },
                values: new object[,]
                {
                    { new Guid("12345678-1234-1234-1234-123456789012"), "bus", "Transport" },
                    { new Guid("550e8400-e29b-41d4-a716-446655440000"), "wallet", "Transfer" },
                    { new Guid("a1b2c3d4-e5f6-4a5b-bc6d-7e8f9a0b1c2d"), "smartphone", "Electronics" },
                    { new Guid("d4c3b2a1-f6e5-4b5a-ac6d-9f8e7d6c5b4a"), "pill", "Pharmacy" },
                    { new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), "utensils", "Food" }
                });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "AccountId", "Amount", "CategoryId", "Date", "Description" },
                values: new object[,]
                {
                    { new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"), new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), -150.50m, new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"), new DateTime(2024, 5, 20, 10, 0, 0, 0, DateTimeKind.Utc), "Grocery" },
                    { new Guid("a555e888-4444-4444-4444-111122223333"), new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), -45.00m, new Guid("d4c3b2a1-f6e5-4b5a-ac6d-9f8e7d6c5b4a"), new DateTime(2024, 5, 22, 9, 30, 0, 0, DateTimeKind.Utc), "Pharmacy" },
                    { new Guid("d290f1ee-6c89-4b20-bc5e-333333333333"), new Guid("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2"), 3000.00m, new Guid("550e8400-e29b-41d4-a716-446655440000"), new DateTime(2024, 5, 21, 12, 0, 0, 0, DateTimeKind.Utc), "Transfer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
