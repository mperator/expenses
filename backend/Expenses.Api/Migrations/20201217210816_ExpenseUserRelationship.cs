using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Api.Migrations
{
    public partial class ExpenseUserRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExpenseUsers",
                columns: table => new
                {
                    ExpenseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseUsers", x => new { x.ExpenseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ExpenseUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExpenseUsers_ExpenseData_ExpenseId",
                        column: x => x.ExpenseId,
                        principalTable: "ExpenseData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseUsers_UserId",
                table: "ExpenseUsers",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExpenseUsers");
        }
    }
}
