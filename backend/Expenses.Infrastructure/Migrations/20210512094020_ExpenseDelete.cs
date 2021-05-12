using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Infrastructure.Migrations
{
    public partial class ExpenseDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Event_EventId",
                table: "Expense");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Event_EventId",
                table: "Expense",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Event_EventId",
                table: "Expense");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Event_EventId",
                table: "Expense",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
