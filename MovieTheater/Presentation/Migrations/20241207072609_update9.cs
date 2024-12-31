using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class update9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Schedule_Seat",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Seat_AccountId",
                table: "Schedule_Seat",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Schedule_Seat_Account_AccountId",
                table: "Schedule_Seat",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Schedule_Seat_Account_AccountId",
                table: "Schedule_Seat");

            migrationBuilder.DropIndex(
                name: "IX_Schedule_Seat_AccountId",
                table: "Schedule_Seat");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Schedule_Seat");
        }
    }
}
