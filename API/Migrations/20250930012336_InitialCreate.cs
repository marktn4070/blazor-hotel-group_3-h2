using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Road = table.Column<string>(type: "text", nullable: false),
                    Zip = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<int>(type: "integer", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    OpenedAt = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    ClosedAt = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    CheckInFrom = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    CheckInUntil = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    CheckOutUntil = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    PercentagePrice = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roomtypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    NumberOfBeds = table.Column<int>(type: "integer", nullable: false),
                    PricePerNight = table.Column<double>(type: "double precision", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roomtypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Facilities",
                columns: table => new
                {
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    Pool = table.Column<bool>(type: "boolean", nullable: false),
                    Fitness = table.Column<bool>(type: "boolean", nullable: false),
                    Restaurant = table.Column<bool>(type: "boolean", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Facilities", x => x.HotelId);
                    table.ForeignKey(
                        name: "FK_Facilities_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Phone = table.Column<int>(type: "integer", nullable: true),
                    HashedPassword = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<string>(type: "text", nullable: true),
                    LastLogin = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PasswordBackdoor = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoomNumber = table.Column<int>(type: "integer", nullable: false),
                    HotelId = table.Column<int>(type: "integer", nullable: false),
                    RoomtypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rooms_Roomtypes_RoomtypeId",
                        column: x => x.RoomtypeId,
                        principalTable: "Roomtypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoomId = table.Column<int>(type: "integer", nullable: false),
                    FinalPrice = table.Column<double>(type: "double precision", nullable: true),
                    BookingStatus = table.Column<int>(type: "integer", nullable: false),
                    Crib = table.Column<bool>(type: "boolean", nullable: false),
                    ExtraBeds = table.Column<int>(type: "integer", nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    HotelId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Hotels_HotelId",
                        column: x => x.HotelId,
                        principalTable: "Hotels",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Hotels",
                columns: new[] { "Id", "CheckInFrom", "CheckInUntil", "CheckOutUntil", "City", "ClosedAt", "Country", "CreatedAt", "Description", "Email", "Name", "OpenedAt", "PercentagePrice", "Phone", "Road", "UpdatedAt", "Zip" },
                values: new object[,]
                {
                    { 1, new TimeOnly(16, 0, 0), new TimeOnly(21, 0, 0), new TimeOnly(10, 0, 0), "Viborg", new TimeOnly(21, 30, 0), "Danmark", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.", "mercantec@mercantec.dk", "Hotel 1", new TimeOnly(9, 0, 0), 0.0, 12345678, "H. C. Andersens Vej 9", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "8800" },
                    { 2, new TimeOnly(16, 0, 0), new TimeOnly(21, 0, 0), new TimeOnly(10, 0, 0), "Viborg", new TimeOnly(21, 30, 0), "Danmark", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.", "mercantec@mercantec.dk", "Hotel 2", new TimeOnly(9, 0, 0), 0.10000000000000001, 12345678, "H. C. Andersens Vej 9", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "8800" },
                    { 3, new TimeOnly(16, 0, 0), new TimeOnly(21, 0, 0), new TimeOnly(10, 0, 0), "Viborg", new TimeOnly(21, 30, 0), "Danmark", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.", "mercantec@mercantec.dk", "Hotel 3", new TimeOnly(9, 0, 0), 0.20000000000000001, 12345678, "H. C. Andersens Vej 9", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "8800" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "User", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "CleaningStaff", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Reception", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Admin", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Roomtypes",
                columns: new[] { "Id", "CreatedAt", "Description", "Name", "NumberOfBeds", "PricePerNight", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Et enkeltværelse med én seng, ideelt til én person.", "Enkeltværelse", 2, 2999.9899999999998, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Et dobbeltværelse med to senge eller en dobbeltseng.", "Dobbeltværelse", 4, 3299.9899999999998, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "En suite med ekstra plads og komfort, ofte med separat opholdsområde.", "Suite", 7, 3399.9899999999998, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Et værelse med plads til hele familien, typisk med flere senge.", "Familieværelse", 8, 3499.9899999999998, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Et deluxe værelse med ekstra faciliteter og komfort.", "Deluxe værelse", 10, 3599.9899999999998, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "Et værelse designet til gæster med særlige behov og nem adgang.", "Handicapvenligt værelse", 5, 3199.9899999999998, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Rooms",
                columns: new[] { "Id", "CreatedAt", "HotelId", "RoomNumber", "RoomtypeId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 101, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 102, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 103, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 104, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 105, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 106, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 107, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 108, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 109, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 110, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 111, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 112, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 13, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 113, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 14, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 114, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 15, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 115, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 16, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 116, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 17, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 117, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 18, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 118, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 19, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 119, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 20, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 120, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 21, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 201, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 22, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 202, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 23, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 203, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 24, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 204, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 25, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 205, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 26, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 206, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 27, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 207, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 28, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 208, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 29, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 209, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 30, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 210, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 31, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 211, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 32, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 212, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 33, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 213, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 34, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 214, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 35, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 215, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 36, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 216, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 37, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 217, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 38, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 218, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 39, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 219, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 40, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 220, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 41, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 301, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 42, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 302, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 43, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 303, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 44, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 304, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 45, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 305, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 46, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 306, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 47, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 307, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 48, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 308, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 49, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 309, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 50, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 310, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 51, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 311, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 52, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 312, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 53, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 313, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 54, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 314, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 55, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 315, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 56, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 316, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 57, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 317, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 58, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 318, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 59, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 319, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 60, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 320, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 61, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 101, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 62, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 102, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 63, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 103, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 64, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 104, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 65, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 105, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 66, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 106, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 67, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 107, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 68, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 108, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 69, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 109, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 70, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 110, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 71, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 111, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 72, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 112, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 73, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 113, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 74, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 114, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 75, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 115, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 76, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 116, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 77, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 117, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 78, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 118, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 79, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 119, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 80, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 120, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 81, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 121, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 82, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 122, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 83, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 123, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 84, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 124, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 85, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 125, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 86, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 126, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 87, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 127, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 88, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 128, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 89, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 129, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 90, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 130, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 91, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 131, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 92, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 132, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 93, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 133, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 94, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 134, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 95, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 135, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 96, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 136, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 97, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 137, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 98, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 201, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 99, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 202, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 100, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 203, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 101, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 204, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 102, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 205, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 103, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 206, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 104, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 207, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 105, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 208, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 106, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 209, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 107, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 210, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 108, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 211, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 109, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 212, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 110, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 213, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 111, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 214, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 112, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 215, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 113, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 216, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 114, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 217, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 115, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 218, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 116, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 219, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 117, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 220, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 118, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 221, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 119, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 222, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 120, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 223, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 121, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 224, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 122, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 225, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 123, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 226, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 124, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 227, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 125, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 228, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 126, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 229, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 127, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 230, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 128, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 231, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 129, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 232, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 130, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 233, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 131, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 234, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 132, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 235, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 133, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 236, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 134, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 237, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 135, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 301, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 136, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 302, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 137, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 303, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 138, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 304, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 139, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 305, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 140, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 306, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 141, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 307, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 142, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 308, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 143, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 309, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 144, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 310, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 145, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 311, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 146, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 312, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 147, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 313, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 148, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 314, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 149, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 315, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 150, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 316, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 151, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 317, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 152, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 318, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 153, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 319, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 154, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 320, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 155, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 321, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 156, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 322, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 157, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 323, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 158, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 324, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 159, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 325, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 160, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 326, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 161, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 327, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 162, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 328, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 163, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 329, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 164, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 330, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 165, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 331, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 166, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 332, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 167, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 333, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 168, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 334, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 169, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 335, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 170, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 336, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 171, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 101, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 172, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 102, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 173, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 103, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 174, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 104, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 175, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 105, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 176, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 106, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 177, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 107, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 178, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 108, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 179, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 109, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 180, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 110, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 181, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 111, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 182, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 112, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 183, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 113, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 184, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 114, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 185, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 115, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 186, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 116, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 187, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 117, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 188, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 118, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 189, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 119, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 190, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 120, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 191, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 121, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 192, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 122, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 193, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 123, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 194, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 124, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 195, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 125, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 196, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 201, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 197, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 202, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 198, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 203, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 199, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 204, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 200, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 205, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 201, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 206, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 202, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 207, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 203, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 208, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 204, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 209, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 205, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 210, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 206, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 211, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 207, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 212, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 208, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 213, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 209, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 214, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 210, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 215, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 211, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 216, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 212, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 217, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 213, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 218, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 214, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 219, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 215, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 220, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 216, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 221, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 217, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 222, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 218, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 223, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 219, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 224, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 220, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 225, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 221, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 301, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 222, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 302, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 223, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 303, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 224, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 304, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 225, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 305, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 226, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 306, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 227, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 307, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 228, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 308, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 229, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 309, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 230, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 310, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 231, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 311, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 232, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 312, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 233, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 313, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 234, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 314, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 235, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 315, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 236, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 316, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 237, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 317, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 238, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 318, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 239, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 319, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 240, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 320, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 241, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 321, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 242, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 322, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 243, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 323, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 244, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 324, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 245, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 325, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "FirstName", "HashedPassword", "LastLogin", "LastName", "PasswordBackdoor", "Phone", "RoleId", "Salt", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "test@test.com", "test", "$2a$11$BJtEDbA0yeNpnSNKPeGh7eCmVA6tIUoC.QLBFqMjGh.7MWUSGtKJe", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "test", "!MyVerySecureSecretKeyThatIsAtLeast32CharactersLong123456789", 12345678, 4, null, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HotelId",
                table: "Bookings",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomId",
                table: "Bookings",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_HotelId",
                table: "Rooms",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_RoomtypeId",
                table: "Rooms",
                column: "RoomtypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Facilities");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "Roomtypes");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
