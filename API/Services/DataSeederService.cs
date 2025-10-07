using API.Data;
using Bogus;
using DomainModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace API.Services;

/// <summary>
/// Service til at seede databasen med test data ved hjælp af Bogus faker library.
/// Genererer realistiske test data for brugere, hoteller, rum og bookinger.
/// </summary>
public class DataSeederService
{
    private readonly AppDBContext _context;
    private readonly ILogger<DataSeederService> _logger;

    public DataSeederService(AppDBContext context, ILogger<DataSeederService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Seeder databasen med komplet test data.
    /// </summary>
    /// <param name="userCount">Antal brugere at oprette</param>
    /// <param name="hotelCount">Antal hoteller at oprette</param>
    /// <param name="roomsPerHotel">Antal rum per hotel</param>
    /// <param name="bookingCount">Antal bookinger at oprette</param>
    public async Task<string> SeedDatabaseAsync(int userCount = 50, int hotelCount = 10, int roomsPerHotel = 20, int bookingCount = 100)
    {
        try
        {
            var summary = new StringBuilder();
            _logger.LogInformation("Starter database seeding...");

            // Tjek om der allerede er data
            var existingUsers = await _context.Users.CountAsync();
            var existingHotels = await _context.Hotels.CountAsync();
            var existingRooms = await _context.Rooms.CountAsync();
            var existingBookings = await _context.Bookings.CountAsync();

            summary.AppendLine($"Eksisterende data før seeding:");
            summary.AppendLine($"- Brugere: {existingUsers}");
            summary.AppendLine($"- Hoteller: {existingHotels}");
            summary.AppendLine($"- Rum: {existingRooms}");
            summary.AppendLine($"- Bookinger: {existingBookings}");
            summary.AppendLine();


            //// Sikr at der findes roller i databasen
            await EnsureRolesExistAsync();
            summary.AppendLine("✅ Roller sikret");

            //// Seed brugere
            var users = await SeedUsersAsync(userCount);
            summary.AppendLine($"✅ Oprettet {users.Count} brugere");

            // Seed hoteller
            var hotels = await SeedHotelsAsync(hotelCount);
            summary.AppendLine($"✅ Oprettet {hotels.Count} hoteller");

            //// Hent eksisterende rumtyper
            var roomtypes = await _context.Roomtypes.ToListAsync();
            summary.AppendLine($"✅ Hentet {roomtypes.Count} rumtyper");

            // Seed rum
            var rooms = await SeedRoomsAsync(hotels, roomtypes, roomsPerHotel);
            summary.AppendLine($"✅ Oprettet {rooms.Count} rum");

            //// Hent eksisterende bookinger 
            var existingBookingsList = await _context.Bookings.ToListAsync();
            summary.AppendLine($"✅ Hentet {existingBookingsList.Count} bookinger");

            //// Hent eksisterende hoteller 
            var existingHotelssList = await _context.Hotels.ToListAsync();
            summary.AppendLine($"✅ Hentet {existingHotelssList.Count} hoteller");

            //// Seed bookinger
            var bookings = await SeedBookingsAsync(existingBookingsList, existingHotelssList, users, rooms, roomtypes, bookingCount);
            summary.AppendLine($"✅ Oprettet {bookings.Count} bookinger");

            summary.AppendLine();
            summary.AppendLine("🎉 Database seeding fuldført succesfuldt!");

            _logger.LogInformation("Database seeding fuldført succesfuldt");
            return summary.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl under database seeding");
            throw;
        }
    }


    /// <summary>
    /// Tjekker at de nødvendige roller eksisterer i databasen.
    /// </summary>
    private async Task EnsureRolesExistAsync()
    {
        var existingRoles = await _context.Roles.ToListAsync();

        _logger.LogInformation("Fundet {RoleCount} roller i databasen: {RoleNames}",
            existingRoles.Count,
            string.Join(", ", existingRoles.Select(r => r.Name)));

        // Tjek om User og Admin roller eksisterer
        var userRole = existingRoles.FirstOrDefault(r => r.Name == "User");
        var adminRole = existingRoles.FirstOrDefault(r => r.Name == "Admin");

        if (userRole == null)
        {
            throw new InvalidOperationException("User rolle ikke fundet i databasen. Kør database migrations først.");
        }

        if (adminRole == null)
        {
            throw new InvalidOperationException("Admin rolle ikke fundet i databasen. Kør database migrations først.");
        }
    }


    /// <summary>
    /// Opretter fake brugere med forskellige roller.
    /// </summary>
    private async Task<List<User>> SeedUsersAsync(int count)
    {
        var existingUsers = await _context.Users.Select(u => u.Email).ToListAsync();
        var users = new List<User>();

        // Hent faktiske rolle ID'er fra databasen
        var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
        var cleaningStaffRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "CleaningStaff");
        var receptionRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Reception");
        var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");

        if (userRole == null || cleaningStaffRole == null || receptionRole == null || adminRole == null)
        {
            throw new InvalidOperationException("User eller Admin rolle ikke fundet i databasen");
        }

        // Danske navne til at gøre data mere realistisk
        var danishFirstNames = new[]
        {
            "Anders", "Anne", "Bo", "Birgitte", "Christian", "Charlotte", "Daniel", "Dorthe",
            "Erik", "Eva", "Frederik", "Freja", "Henrik", "Helle", "Jacob", "Janne",
            "Klaus", "Karen", "Lars", "Lone", "Mads", "Maria", "Niels", "Nina",
            "Ole", "Pia", "Peter", "Rikke", "Søren", "Susanne", "Thomas", "Tina"
        };

        var danishLastNames = new[]
        {
            "Andersen", "Nielsen", "Hansen", "Pedersen", "Jørgensen", "Larsen", "Sørensen",
            "Rasmussen", "Petersen", "Christensen", "Thomsen", "Olsen", "Madsen", "Møller",
            "Johansen", "Christiansen", "Jensen", "Kristensen", "Knudsen", "Mortensen"
        };

        var faker = new Faker<User>("en")
            //.RuleFor(u => u.Email, f =>
            //{
            //    var firstName = f.PickRandom(danishFirstNames);
            //    var lastName = f.PickRandom(danishLastNames);
            //    return $"{firstName.ToLower()}.{lastName.ToLower()}@{f.PickRandom("gmail.com", "hotmail.com", "yahoo.dk", "outlook.dk")}";
            //})
            .RuleFor(u => u.FirstName, f => f.PickRandom(danishFirstNames))
            .RuleFor(u => u.LastName, f => f.PickRandom(danishLastNames))
            .RuleFor(u => u.Email, (f, u) => $"{u.FirstName.ToLower()}.{u.LastName.ToLower()}@{f.PickRandom("gmail.com", "hotmail.com", "yahoo.dk", "outlook.dk")}")
            .RuleFor(u => u.Phone, f => f.Random.Int(20000000, 99999999))
            .RuleFor(u => u.HashedPassword, f => HashPassword("Password123!"))
            .RuleFor(u => u.PasswordBackdoor, f => "Password123!")
            .RuleFor(u => u.RoleId, f => f.PickRandom(userRole.Id, userRole.Id, userRole.Id, userRole.Id, cleaningStaffRole.Id, cleaningStaffRole.Id, cleaningStaffRole.Id, receptionRole.Id, receptionRole.Id, adminRole.Id)) // 40% User, 30% Admin, 20% Admin, 10% Admin
            .RuleFor(u => u.LastLogin, f => f.Date.Between(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow.AddHours(2)))
            .RuleFor(u => u.CreatedAt, f => f.Date.Between(DateTime.UtcNow.AddYears(-2), DateTime.UtcNow.AddHours(2)))
            .RuleFor(u => u.UpdatedAt, (f, u) => f.Date.Between(u.CreatedAt, DateTime.UtcNow.AddHours(2)));

        // Generer brugere og filtrér dubletter
        var attempts = 0;
        while (users.Count < count && attempts < count * 2)
        {
            var user = faker.Generate();
            if (!existingUsers.Contains(user.Email) && !users.Any(u => u.Email == user.Email))
            {
                users.Add(user);
            }
            attempts++;
        }

        // Tilføj en garanteret admin bruger
        if (!existingUsers.Contains("admin@hotel.dk"))
        {
            users.Add(new User
            {
                Email = "admin@hotel.dk",
                Phone = 12345678,
                FirstName = "admin",
                LastName = "admin",
                HashedPassword = HashPassword("Admin123!"),
                PasswordBackdoor = "Admin123!",
                RoleId = adminRole.Id,
                LastLogin = DateTime.UtcNow.AddDays(-1),
                CreatedAt = DateTime.UtcNow.AddDays(-365),
                UpdatedAt = DateTime.UtcNow.AddDays(-1)
            });
        }

        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();

        return users;
    }


    /// <summary>
    /// Opretter fake hoteller med realistiske data.
    /// </summary>
    private async Task<List<Hotel>> SeedHotelsAsync(int count)
    {
        var hotels = new List<Hotel>();

        var hotelNames = new[]
        {
            "Hotel Royal", "Grand Hotel", "Scandic", "Best Western", "Radisson Blu",
            "Hotel Alexandra", "Nimb Hotel", "Hotel d'Angleterre", "Copenhagen Marriott",
            "Clarion Hotel", "Comfort Hotel", "First Hotel", "Cabinn Hotel", "Wakeup Hotel",
            "Hotel Phoenix", "Hotel Kong Arthur", "Hotel Sanders", "71 Nyhavn Hotel",
            "Hotel Skt. Petri", "AC Hotel", "Villa Copenhagen", "Hotel SP34"
        };

        var emailDomainNames = new[]
        {
            // Danske domæner
            "live.dk","mail.dk","posteo.dk","privat.dk","stofanet.dk","youmail.dk",
            "jubii.dk","tdc.dk","get2net.dk","mailme.dk","firma.dk",
            // Udenlandske domæner
            "gmail.com","hotmail.com","outlook.com","yahoo.com","icloud.com","aol.com","protonmail.com","gmx.com","zoho.com",
            "mail.com","yandex.com","fastmail.com","outlook.de","orange.fr","web.de","bluewin.ch","btinternet.com","comcast.net"
        };

        var danishCities = new[]
        {
            "København", "Aarhus", "Odense", "Aalborg", "Esbjerg",
            "Randers", "Kolding", "Horsens", "Vejle", "Roskilde",
            "Herning", "Silkeborg", "Næstved", "Fredericia", "Viborg"
        };

        var danishStreets = new[]
        {
            "Nørregade", "Vestergade", "Østergade", "Søndergade", "Hovedgade",
            "Kongens Nytorv", "Strøget", "Nyhavn", "Amaliegade", "Bredgade",
            "Store Kongensgade", "Gothersgade", "Sankt Peders Stræde"
        };

        for (int i = 0; i < count; i++)
        {
            var baseFaker = new Faker();

            /// <summary>
            /// Omdanner strenge til gyldig hotel navne værdier.
            /// </summary>
            var hotelName = baseFaker.PickRandom(hotelNames) + " " + baseFaker.PickRandom(danishCities);

            // Sikr unikt navn
            var counter = 1;
            var originalName = hotelName;
            while (hotels.Any(h => h.Name == hotelName))
            {
                hotelName = originalName + " " + counter;
                counter++;
            }

            // Erstat danske specialtegn med ae, oe, aa (både små og store bogstaver)
            hotelName = hotelName
                .Replace("æ", "ae")
                .Replace("Æ", "Ae")
                .Replace("ø", "oe")
                .Replace("Ø", "Oe")
                .Replace("å", "aa")
                .Replace("Å", "Aa");

            // Fjern alle tegn der ikke er bogstaver, tal eller mellemrum
            var hotelNameCleaned = Regex.Replace(hotelName, @"[^a-zA-Z0-9 ]", "");

            /// <summary>
            /// Omdanner decimaler til gyldig hotel PercentagePrice værdier.
            /// </summary>
            double PercentagePrice;

            double lowRangeNum = 0.1;
            double highRangeNum = 1.9;

            PercentagePrice = baseFaker.Random.WeightedRandom(new[] { 0.4, 0.8, 1.2, 1.6, 1.9 }, new[] { 0.05f, 0.25f, 0.4f, 0.25f, 0.05f });

            (lowRangeNum, highRangeNum) = PercentagePrice switch
            {
                0.4 => (0.1, 0.4),
                0.8 => (0.4, 0.8),
                1.2 => (0.8, 1.2),
                1.6 => (1.2, 1.6),
                1.9 => (1.6, 1.9),
                // Default hvis ingen af de andre matcher
                _ => (lowRangeNum, highRangeNum)
            };

            /// <summary>
            /// Omdanner int til gyldig hotel time værdier.
            /// </summary>
            int estimatedCleaningHours = baseFaker.Random.Int(1, 2);

            TimeOnly opened = new TimeOnly(baseFaker.Random.Int(7, 10), 0, 0);

            // Finder mindste værdi mellem (åbningstid + tilfældige timer) og (23)
            TimeOnly closed = opened.AddHours(Math.Min(opened.Hour + baseFaker.Random.Int(13, 16), 23));

            // Finder mindste værdi mellem (åbningstid + tilfældige timer) og (11)
            TimeOnly checkOutUntil = opened.AddHours(Math.Min(opened.Hour + baseFaker.Random.Int(1, 2), 11));

            var hotel = new Hotel
            {
                Name = hotelName,
                Road = baseFaker.PickRandom(danishStreets) + " " + baseFaker.Random.Int(1, 200),
                Zip = baseFaker.Random.Int(1000, 9999).ToString(),
                City = baseFaker.PickRandom(danishCities),
                Country = "Danmark",
                Phone = baseFaker.Random.Int(20000000, 99999999),
                Email = hotelNameCleaned.ToLower().Replace(" ", "") + "@" + baseFaker.PickRandom(emailDomainNames),
                Description = baseFaker.Lorem.Paragraphs(1, 2),
                OpenedAt = opened,
                ClosedAt = closed,
                CheckInFrom = checkOutUntil,
                CheckInUntil = checkOutUntil.AddHours(estimatedCleaningHours),
                CheckOutUntil = closed.AddHours(baseFaker.Random.Int(-2, -1)),
                PercentagePrice = Math.Round(baseFaker.Random.Double(lowRangeNum, highRangeNum), 2),
                CreatedAt = baseFaker.Date.Between(DateTime.UtcNow.AddYears(-5), DateTime.UtcNow.AddYears(-1)),
                UpdatedAt = DateTime.UtcNow.AddHours(2),
            };

            hotel.UpdatedAt = baseFaker.Date.Between(hotel.CreatedAt, DateTime.UtcNow);
            hotels.Add(hotel);
        }

        _context.Hotels.AddRange(hotels);
        await _context.SaveChangesAsync();

        return hotels;
    }

    /// <summary>
    /// Opretter fake rum for hvert hotel.
    /// </summary>
    private async Task<List<Room>> SeedRoomsAsync(List<Hotel> hotels, List<Roomtype> roomtypes, int roomsPerHotel)
    {
        var rooms = new List<Room>();

        // Hvis ingen rumtyper i DB, kast eller håndter
        if (roomtypes == null || !roomtypes.Any())
            throw new InvalidOperationException("Ingen roomtypes fundet i databasen. Seed roomtypes først.");

        // Array af roomtypeId's 
        var roomtypeIds = roomtypes.Select(rt => rt.Id).ToArray();


        foreach (var hotel in hotels)
        {
            var faker = new Faker<Room>("en")
                .RuleFor(r => r.RoomNumber, f => f.Random.Int(101, 999))
                //.RuleFor(r => r.NumberOfBeds, f => f.Random.WeightedRandom(new[] { 1, 2, 3, 4, 6 }, new[] { 0.1f, 0.5f, 0.2f, 0.15f, 0.05f }))
                .RuleFor(r => r.RoomtypeId, f => f.PickRandom(roomtypeIds))
                .RuleFor(r => r.HotelId, f => hotel.Id)
                .RuleFor(r => r.CreatedAt, f => f.Date.Between(hotel.CreatedAt, DateTime.UtcNow.AddHours(2)))
                .RuleFor(r => r.UpdatedAt, (f, r) => f.Date.Between(r.CreatedAt, DateTime.UtcNow.AddHours(2)));

            var hotelRooms = faker.Generate(roomsPerHotel);

            // Sikr unikke rum numre per hotel
            var usedNumbers = new HashSet<int>();
            foreach (var room in hotelRooms)
            {
                while (usedNumbers.Contains(room.RoomNumber))
                {
                    room.RoomNumber = new Faker().Random.Int(101, 999);
                }
                usedNumbers.Add(room.RoomNumber);
            }

            rooms.AddRange(hotelRooms);
        }

        _context.Rooms.AddRange(rooms);
        await _context.SaveChangesAsync();

        return rooms;
    }


    /// <summary>
    /// Opretter fake bookinger med realistiske datoer og ingen overlaps.
    /// </summary>
    private async Task<List<Booking>> SeedBookingsAsync(List<Booking> db_bookings, List<Hotel> db_hotels, List<User> db_users, List<Room> db_rooms, List<Roomtype> db_roomtypes, int count)
    {
        var bookings = new List<Booking>();
        var bookingStatuses = new[] {
            BookingStatus.Confirmed,
            BookingStatus.Cancelled,
            BookingStatus.CheckedIn,
            BookingStatus.CheckedOut
        };
        var faker = new Faker("en");

        // Opret bookinger med forsigtig overlap håndtering
        for (int i = 0; i < count; i++)
        {
            var user = faker.PickRandom(db_users);
            var room = faker.PickRandom(db_rooms);
            var hotel = db_hotels.FirstOrDefault(h => h.Id == room.HotelId);
            var roomtype = db_roomtypes.FirstOrDefault(rt => rt.Id == room.RoomtypeId);

            // Generer realistiske datoer
            var startDate = faker.Date.Between(
                DateTime.UtcNow.AddHours(2).AddDays(-180),
                DateTime.UtcNow.AddHours(2).AddDays(180)
            );

            var nights = faker.Random.WeightedRandom(
                new[] { 1, 2, 3, 4, 5, 7, 14 },
                new[] { 0.1f, 0.3f, 0.25f, 0.15f, 0.1f, 0.08f, 0.02f }
            );
            var endDate = startDate.AddDays(nights);

            // Tjek overlap ud fra allerede genererede bookinger
            bool overlapWithNew = bookings.Any(b =>
                b.RoomId == room.Id &&
                b.BookingStatus != BookingStatus.Cancelled &&
                // Check om b_list_StartDate er før random_EndDate
                b.StartDate < endDate &&
                // Check om b_list_EndDate er efter random_StartDate
                b.EndDate > startDate);
            // true hvis: b_list_StartDate -> random_StartDate -> random_EndDate -> b_list_EndDate
            // true hvis: random_StartDate -> b_list_StartDate -> b_list_EndDate -> random_EndDate
            // true hvis: random_StartDate -> b_list_StartDate -> random_EndDate -> b_list_EndDate
            // true hvis: b_list_StartDate -> random_StartDate -> b_list_EndDate -> random_EndDate

            // Tjek overlap ud fra allerede eksisterende bookinger i DB
            bool overlapWithDb = db_bookings != null && db_bookings.Any(db_b =>
                db_b.RoomId == room.Id &&
                db_b.BookingStatus != BookingStatus.Cancelled &&
                db_b.StartDate < endDate &&
                db_b.EndDate > startDate);

            // Tjek for overlap
            if (!overlapWithNew && !overlapWithDb)
            {
                var booking = new Booking
                {
                    UserId = user.Id,
                    RoomId = room.Id,
                    StartDate = startDate,
                    EndDate = endDate,
                    //NumberOfBeds = faker.Random.Int(1, Math.Min(room.Capacity, 4)),
                    FinalPrice = Math.Round(roomtype.PricePerNight * nights * hotel.PercentagePrice, 2),
                    BookingStatus = faker.PickRandom(bookingStatuses),
                    //SpecialRequests = faker.Random.Bool(0.3f) ? faker.Lorem.Sentence() : null,
                    Crib = faker.Random.Bool(),
                    ExtraBeds = faker.Random.Int(1, 2),
                    CreatedAt = faker.Date.Between(startDate.AddDays(-30), startDate.AddDays(-1)),
                    UpdatedAt = faker.Date.Between(startDate.AddDays(-10), DateTime.UtcNow.AddHours(2))
                };

                bookings.Add(booking);
            }
        }

        _context.Bookings.AddRange(bookings);
        await _context.SaveChangesAsync();

        return bookings;
    }



    /// <summary>
    /// Seeder kun bookinger baseret på eksisterende brugere og rum.
    /// </summary>
    /// <param name="bookingCount">Antal bookinger at oprette</param>
    /// <returns>Seeding resultat</returns>
    public async Task<string> SeedBookingsOnlyAsync(int bookingCount = 50)
    {
        try
        {
            var summary = new StringBuilder();
            _logger.LogInformation("Starter booking-only seeding...");

            // Hent eksisterende brugere og rum
            var existingBookings = await _context.Bookings.ToListAsync();
            var existingHotels = await _context.Hotels.ToListAsync();
            var existingUsers = await _context.Users.ToListAsync();
            var existingRooms = await _context.Rooms.ToListAsync();
            var existingRoomtypes = await _context.Roomtypes.ToListAsync();


            if (!existingBookings.Any())
            {
                throw new InvalidOperationException("Ingen bookninger fundet i databasen. Seed bookninger først.");
            }

            if (!existingHotels.Any())
            {
                throw new InvalidOperationException("Ingen hoteller fundet i databasen. Seed hoteller først.");
            }

            if (!existingUsers.Any())
            {
                throw new InvalidOperationException("Ingen brugere fundet i databasen. Seed brugere først.");
            }

            if (!existingRooms.Any())
            {
                throw new InvalidOperationException("Ingen rum fundet i databasen. Seed hoteller og rum først.");
            }

            if (!existingRoomtypes.Any())
            {
                throw new InvalidOperationException("Ingen rumtyper fundet i databasen. Seed hoteller og rumtyper først.");
            }

            summary.AppendLine($"Fundet {existingBookings.Count} bookninger, {existingHotels.Count} hoteller, {existingUsers.Count} brugere, {existingRooms.Count} rum og {existingRoomtypes.Count} rumtyper");

            // Seed bookinger
            var bookings = await SeedBookingsAsync(existingBookings, existingHotels, existingUsers, existingRooms, existingRoomtypes, bookingCount);
            summary.AppendLine($"✅ Oprettet {bookings.Count} nye bookinger");

            summary.AppendLine();
            summary.AppendLine("🎉 Booking seeding fuldført succesfuldt!");

            _logger.LogInformation("Booking seeding fuldført succesfuldt");
            return summary.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl under booking seeding");
            throw;
        }
    }

    /// <summary>
    /// Rydder alle data fra databasen.
    /// </summary>
    public async Task<string> ClearDatabaseAsync()
    {
        try
        {
            _logger.LogInformation("Rydder database...");

            var bookingCount = await _context.Bookings.CountAsync();
            var roomCount = await _context.Rooms.CountAsync();
            var hotelCount = await _context.Hotels.CountAsync();
            var userCount = await _context.Users.CountAsync();

            _context.Bookings.RemoveRange(_context.Bookings);
            _context.Rooms.RemoveRange(_context.Rooms);
            _context.Hotels.RemoveRange(_context.Hotels);
            _context.Users.RemoveRange(_context.Users);

            await _context.SaveChangesAsync();

            var summary = $"🗑️ Database ryddet!\n" +
                            $"- Slettet {bookingCount} bookinger\n" +
                            $"- Slettet {roomCount} rum\n" +
                            $"- Slettet {hotelCount} hoteller\n" +
                            $"- Slettet {userCount} brugere";

            _logger.LogInformation("Database ryddet succesfuldt");
            return summary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Fejl ved rydning af database");
            throw;
        }
    }

    /// <summary>
    /// Henter database statistikker.
    /// </summary>
    public async Task<object> GetDatabaseStatsAsync()
    {
        var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
        var adminCount = adminRole != null ? await _context.Users.CountAsync(u => u.RoleId == adminRole.Id) : 0;

        return new
        {
            Users = await _context.Users.CountAsync(),
            AdminUsers = adminCount,
            Hotels = await _context.Hotels.CountAsync(),
            Rooms = await _context.Rooms.CountAsync(),
            Bookings = await _context.Bookings.CountAsync(),
            ActiveBookings = await _context.Bookings.CountAsync(b => b.BookingStatus == BookingStatus.Confirmed || b.BookingStatus == BookingStatus.CheckedIn),
            LastSeeded = DateTime.UtcNow.AddHours(2)
        };
    }

    /// <summary>
    /// Hash password helper metode.
    /// </summary>
    private static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}
