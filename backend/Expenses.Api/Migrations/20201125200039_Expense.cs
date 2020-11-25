﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Expenses.Api.Migrations
{
    public partial class Expense : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "EventData");

            migrationBuilder.AddColumn<string>(
                name: "CreatorId",
                table: "EventData",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExpenseData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<float>(type: "real", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IssuerId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpenseData_AspNetUsers_IssuerId",
                        column: x => x.IssuerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpenseData_EventData_EventId",
                        column: x => x.EventId,
                        principalTable: "EventData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventData_CreatorId",
                table: "EventData",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseData_EventId",
                table: "ExpenseData",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseData_IssuerId",
                table: "ExpenseData",
                column: "IssuerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventData_AspNetUsers_CreatorId",
                table: "EventData",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventData_AspNetUsers_CreatorId",
                table: "EventData");

            migrationBuilder.DropTable(
                name: "ExpenseData");

            migrationBuilder.DropIndex(
                name: "IX_EventData_CreatorId",
                table: "EventData");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "EventData");

            migrationBuilder.AddColumn<string>(
                name: "Creator",
                table: "EventData",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
