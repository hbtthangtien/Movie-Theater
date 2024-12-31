using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class update2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Schedule_Seat_ScheduleSeatId",
                table: "Ticket");

            migrationBuilder.DropIndex(
                name: "IX_Ticket_ScheduleSeatId",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "ScheduleSeatId",
                table: "Ticket");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "Invoice",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ScheduleSeatId",
                table: "Ticket",
                type: "varchar(10)",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "status",
                table: "Invoice",
                type: "bit",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ScheduleSeatId",
                table: "Ticket",
                column: "ScheduleSeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Schedule_Seat_ScheduleSeatId",
                table: "Ticket",
                column: "ScheduleSeatId",
                principalTable: "Schedule_Seat",
                principalColumn: "schedule_seat_id");
        }
    }
}
