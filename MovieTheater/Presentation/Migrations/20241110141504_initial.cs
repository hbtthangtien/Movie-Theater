using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    date_of_birth = table.Column<DateOnly>(type: "date", nullable: true),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fullname = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    gender = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    identity_card = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    phone_number = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    register_date = table.Column<DateOnly>(type: "date", nullable: true, defaultValueSql: "(getdate())"),
                    Status = table.Column<int>(type: "int", nullable: true),
                    refresh_token = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    refresh_token_expire = table.Column<DateOnly>(type: "date", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cinema_Room",
                columns: table => new
                {
                    cinema_room_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cineme_room_name = table.Column<string>(type: "nvarchar(2555)", maxLength: 2555, nullable: true),
                    seat_quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cinema_Room", x => x.cinema_room_id);
                });

            migrationBuilder.CreateTable(
                name: "Promotion",
                columns: table => new
                {
                    promotion_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    detail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    discount_level = table.Column<int>(type: "int", nullable: true),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: true),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotion", x => x.promotion_id);
                });

            migrationBuilder.CreateTable(
                name: "Schedule",
                columns: table => new
                {
                    schedule_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    schedule_time = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MovieScheduleDate = table.Column<DateOnly>(type: "date", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule", x => x.schedule_id);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    type_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    type_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.type_id);
                });

            migrationBuilder.CreateTable(
                name: "TypeSeat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeSeat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_Account_UserId",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_Account_UserId",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_Account_UserId",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    employee_id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employee", x => x.employee_id);
                    table.ForeignKey(
                        name: "FK_Roles_Employee",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Invoice",
                columns: table => new
                {
                    invoice_id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    add_score = table.Column<double>(type: "float", nullable: true),
                    booking_date = table.Column<DateTime>(type: "datetime2", rowVersion: true, nullable: false),
                    movie_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    schedule_show = table.Column<DateOnly>(type: "date", nullable: true),
                    schedule_show_time = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    total_money = table.Column<decimal>(type: "money", nullable: true),
                    use_score = table.Column<double>(type: "float", nullable: true),
                    seat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoice", x => x.invoice_id);
                    table.ForeignKey(
                        name: "FK_Invoice_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    member_id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    score = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.member_id);
                    table.ForeignKey(
                        name: "FK_Member_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    payment_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payment_date = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    payment_method = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    payment_status = table.Column<int>(type: "int", nullable: true),
                    total_amount = table.Column<double>(type: "float", nullable: true),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment_Id", x => x.payment_id);
                    table.ForeignKey(
                        name: "FK_Payment_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_Account_UserId",
                        column: x => x.UserId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    movie_id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    actor = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    cinema_room_id = table.Column<int>(type: "int", nullable: true),
                    content = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    director = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    duration = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    from_date = table.Column<DateOnly>(type: "date", nullable: true),
                    movie_production_company = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    to_date = table.Column<DateOnly>(type: "date", nullable: true),
                    version = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    movie_name_english = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    movie_name_vn = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    large_image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    small_image = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.movie_id);
                    table.ForeignKey(
                        name: "FK_Cinema_Room_Movie",
                        column: x => x.cinema_room_id,
                        principalTable: "Cinema_Room",
                        principalColumn: "cinema_room_id");
                });

            migrationBuilder.CreateTable(
                name: "Seat",
                columns: table => new
                {
                    seat_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cinema_room_id = table.Column<int>(type: "int", nullable: true),
                    seat_colunm = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    seat_row = table.Column<int>(type: "int", nullable: true),
                    seat_status = table.Column<int>(type: "int", nullable: true),
                    seatType_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seat", x => x.seat_id);
                    table.ForeignKey(
                        name: "FK_Seat_Cinema_Room",
                        column: x => x.cinema_room_id,
                        principalTable: "Cinema_Room",
                        principalColumn: "cinema_room_id");
                    table.ForeignKey(
                        name: "FK_Seat_SeatType",
                        column: x => x.seatType_id,
                        principalTable: "TypeSeat",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransactionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payment_id = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    status = table.Column<int>(type: "int", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionHistory_Payment",
                        column: x => x.payment_id,
                        principalTable: "Payment",
                        principalColumn: "payment_id");
                });

            migrationBuilder.CreateTable(
                name: "Movie_Schedule",
                columns: table => new
                {
                    movie_id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    schedule_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_Schedule", x => new { x.movie_id, x.schedule_id });
                    table.ForeignKey(
                        name: "FK_Movie_Schedule",
                        column: x => x.movie_id,
                        principalTable: "Movie",
                        principalColumn: "movie_id");
                    table.ForeignKey(
                        name: "FK_Schedule_Movie",
                        column: x => x.schedule_id,
                        principalTable: "Schedule",
                        principalColumn: "schedule_id");
                });

            migrationBuilder.CreateTable(
                name: "Movie_Type",
                columns: table => new
                {
                    movie_id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    type_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie_Type", x => new { x.movie_id, x.type_id });
                    table.ForeignKey(
                        name: "FK_Movie_Type",
                        column: x => x.movie_id,
                        principalTable: "Movie",
                        principalColumn: "movie_id");
                    table.ForeignKey(
                        name: "FK_Type_Movie",
                        column: x => x.type_id,
                        principalTable: "Type",
                        principalColumn: "type_id");
                });

            migrationBuilder.CreateTable(
                name: "Schedule_Seat",
                columns: table => new
                {
                    schedule_seat_id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    movie_id = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    schedule_id = table.Column<int>(type: "int", nullable: true),
                    seat_id = table.Column<int>(type: "int", nullable: true),
                    seat_column = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    seat_row = table.Column<int>(type: "int", nullable: true),
                    seat_status = table.Column<int>(type: "int", nullable: true),
                    seatType_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schedule_Seat", x => x.schedule_seat_id);
                    table.ForeignKey(
                        name: "FK_Schedule_Seat_Movie",
                        column: x => x.movie_id,
                        principalTable: "Movie",
                        principalColumn: "movie_id");
                    table.ForeignKey(
                        name: "FK_Schedule_Seat_Schedule",
                        column: x => x.schedule_id,
                        principalTable: "Schedule",
                        principalColumn: "schedule_id");
                    table.ForeignKey(
                        name: "FK_Schedule_Seat_Seat",
                        column: x => x.seat_id,
                        principalTable: "Seat",
                        principalColumn: "seat_id");
                    table.ForeignKey(
                        name: "FK_TypeSeat_ScheduleSeat",
                        column: x => x.seatType_id,
                        principalTable: "TypeSeat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ticket_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    price = table.Column<decimal>(type: "money", nullable: true),
                    ScheduleSeatId = table.Column<string>(type: "varchar(10)", nullable: true),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AccountId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ticket_type = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ticket_id);
                    table.ForeignKey(
                        name: "FK_Ticket_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ticket_Schedule_Seat_ScheduleSeatId",
                        column: x => x.ScheduleSeatId,
                        principalTable: "Schedule_Seat",
                        principalColumn: "schedule_seat_id");
                });

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Account",
                column: "NormalizedEmail",
                unique: true,
                filter: "[NormalizedEmail] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_email",
                table: "Account",
                column: "email",
                unique: true,
                filter: "[email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_identity_card",
                table: "Account",
                column: "identity_card",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Account_refresh_token",
                table: "Account",
                column: "refresh_token",
                unique: true,
                filter: "[refresh_token] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserName",
                table: "Account",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Account",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Employee_AccountId",
                table: "Employee",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_AccountId",
                table: "Invoice",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Member_AccountId",
                table: "Member",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_cinema_room_id",
                table: "Movie",
                column: "cinema_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Schedule_schedule_id",
                table: "Movie_Schedule",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_Movie_Type_type_id",
                table: "Movie_Type",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_AccountId",
                table: "Payment",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Seat_movie_id",
                table: "Schedule_Seat",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Seat_schedule_id",
                table: "Schedule_Seat",
                column: "schedule_id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Seat_seat_id",
                table: "Schedule_Seat",
                column: "seat_id");

            migrationBuilder.CreateIndex(
                name: "IX_Schedule_Seat_seatType_id",
                table: "Schedule_Seat",
                column: "seatType_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_cinema_room_id",
                table: "Seat",
                column: "cinema_room_id");

            migrationBuilder.CreateIndex(
                name: "IX_Seat_seatType_id",
                table: "Seat",
                column: "seatType_id");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_AccountId",
                table: "Ticket",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_ScheduleSeatId",
                table: "Ticket",
                column: "ScheduleSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionHistory_payment_id",
                table: "TransactionHistory",
                column: "payment_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Invoice");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Movie_Schedule");

            migrationBuilder.DropTable(
                name: "Movie_Type");

            migrationBuilder.DropTable(
                name: "Promotion");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "TransactionHistory");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropTable(
                name: "Schedule_Seat");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "Schedule");

            migrationBuilder.DropTable(
                name: "Seat");

            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Cinema_Room");

            migrationBuilder.DropTable(
                name: "TypeSeat");
        }
    }
}
