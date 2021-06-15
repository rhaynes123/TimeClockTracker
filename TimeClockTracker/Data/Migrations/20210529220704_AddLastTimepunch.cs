using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeClockTracker.Data.Migrations
{
    public partial class AddLastTimepunch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastPunch",
                table: "TimePunches",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastPunch",
                table: "TimePunches");
        }
    }
}
