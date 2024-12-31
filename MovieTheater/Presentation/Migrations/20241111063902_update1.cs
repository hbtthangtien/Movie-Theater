using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "Ticket");

            migrationBuilder.AddColumn<string>(
                name: "cinema_room_name",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "movie_name",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "schedule_show",
                table: "Ticket",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "schedule_show_time",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "cinema_room_name",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "movie_name",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "schedule_show",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "schedule_show_time",
                table: "Ticket");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "Ticket",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
