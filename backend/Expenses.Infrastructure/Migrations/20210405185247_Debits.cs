using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Infrastructure.Migrations
{
    public partial class Debits : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Debit",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    DebitorId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Debit", x => new { x.ExpenseId, x.Id });
                    table.ForeignKey(
                        name: "FK_Debit_AspNetUsers_DebitorId",
                        column: x => x.DebitorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Debit_DebitorId",
                table: "Debit",
                column: "DebitorId",
                unique: true,
                filter: "[DebitorId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Debit");
        }
    }
}
