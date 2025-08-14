using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BacktestStudio.Repository.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    Price = table.Column<decimal>(type: "DECIMAL(10, 2)", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    Symbol = table.Column<string>(type: "VARCHAR(10)", nullable: false, defaultValue: "STOCK"),
                    CreatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "DATETIME", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRecords", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_purchase_records_date",
                table: "PurchaseRecords",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "idx_purchase_records_symbol",
                table: "PurchaseRecords",
                column: "Symbol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseRecords");
        }
    }
}
