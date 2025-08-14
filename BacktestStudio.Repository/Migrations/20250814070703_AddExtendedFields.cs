using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BacktestStudio.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddExtendedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ProfitAmount",
                table: "PurchaseRecords",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SettlementDate",
                table: "PurchaseRecords",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "StopLossPrice",
                table: "PurchaseRecords",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfitAmount",
                table: "PurchaseRecords");

            migrationBuilder.DropColumn(
                name: "SettlementDate",
                table: "PurchaseRecords");

            migrationBuilder.DropColumn(
                name: "StopLossPrice",
                table: "PurchaseRecords");
        }
    }
}
