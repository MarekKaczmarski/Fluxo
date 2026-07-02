using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fluxo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountConcurrencyToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Accounts",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Date",
                table: "Transactions",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_Name",
                table: "Accounts",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Transactions_Date",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Name",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_Name",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Accounts");
        }
    }
}
