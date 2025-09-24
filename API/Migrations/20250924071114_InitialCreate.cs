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
                    { 1, new TimeOnly(16, 0, 0), new TimeOnly(21, 0, 0), new TimeOnly(10, 0, 0), "Viborg", new TimeOnly(21, 30, 0), "Danmark", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.", "mercantec@mercantec.dk", "Hotel 1", new TimeOnly(9, 0, 0), 1.0, 12345678, "H. C. Andersens Vej 9", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "8800" },
                    { 2, new TimeOnly(16, 0, 0), new TimeOnly(21, 0, 0), new TimeOnly(10, 0, 0), "Viborg", new TimeOnly(21, 30, 0), "Danmark", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.", "mercantec@mercantec.dk", "Hotel 2", new TimeOnly(9, 0, 0), 1.0, 12345678, "H. C. Andersens Vej 9", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "8800" },
                    { 3, new TimeOnly(16, 0, 0), new TimeOnly(21, 0, 0), new TimeOnly(10, 0, 0), "Viborg", new TimeOnly(21, 30, 0), "Danmark", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.", "mercantec@mercantec.dk", "Hotel 3", new TimeOnly(9, 0, 0), 1.0, 12345678, "H. C. Andersens Vej 9", new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "8800" }
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
                    { 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 104, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 105, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 106, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 7, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 2, 107, 1, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 8, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 108, 2, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 9, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 109, 3, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 10, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 110, 4, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 11, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 111, 5, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 12, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), 3, 112, 6, new DateTime(2025, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc) }
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
