using Bogus;
using DomainModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace API.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<Hotel> Hotels { get; set; } = null!;
        public DbSet<Room> Rooms { get; set; } = null!;
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<Facility> Facilities { get; set; } = null!;
        public DbSet<Roomtype> Roomtypes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Konfigurer Role entity
            modelBuilder.Entity<Role>(entity =>
            {
                // Navn skal være unikt
                entity.HasIndex(r => r.Name).IsUnique();
            });

            // Konfigurer User entity
            modelBuilder.Entity<User>(entity =>
            {
                // Email skal være unikt
                entity.HasIndex(u => u.Email).IsUnique();

                // Konfigurer foreign key til Role
                entity.HasOne(u => u.Role)
                    .WithMany(r => r.Users)
                    .HasForeignKey(u => u.RoleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Facility>()
                .HasKey(f => f.HotelId); // Shared PK

            modelBuilder.Entity<Hotel>()
                .HasOne(h => h.Facility)
                .WithOne(f => f.Hotel)
                .HasForeignKey<Facility>(f => f.HotelId);

            modelBuilder.Entity<Hotel>()
                .HasMany(h => h.Rooms)
                .WithOne(r => r.Hotel)
                .HasForeignKey(r => r.HotelId);

            modelBuilder.Entity<Roomtype>()
                .HasMany(t => t.Rooms)
                .WithOne(r => r.Roomtype)
                .HasForeignKey(r => r.RoomtypeId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId);

            // Seed roller og test brugere (kun til udvikling)
            SeedRoom(modelBuilder);
            SeedRoles(modelBuilder);
            SeedUser(modelBuilder);
            SeedRoomtype(modelBuilder);
            SeedHotel(modelBuilder);
        }

        private void SeedRoom(ModelBuilder modelBuilder)
        {
            var rooms = new List<Room>();
            int roomId = 1;

            int[] totalRoomsPerHotel = { 60, 110, 75 };

            for (int h = 0; h < totalRoomsPerHotel.Length; h++)
            {
                // Antal værelser i det aktuelle hotel
                int roomsInCurrentHotel = totalRoomsPerHotel[h];
                if (roomsInCurrentHotel == 0) continue;

                // Antal værelser fordelt ligeligt på 3 "etager", hvor 3. etage dog kan have færre værelser
                int roomsPerFloor = (int)Math.Ceiling(roomsInCurrentHotel / 3.0);

                for (int r = 0; r < roomsInCurrentHotel; r++)
                {
                    // Beregner etage nummer (int afrundes ned til nærmeste hele tal)
                    int floor = (r / roomsPerFloor) + 1;

                    // Beregner værelse på etagen
                    int roomOnFloor = (r % roomsPerFloor) + 1;

                    rooms.Add(new Room
                    {
                        Id = roomId++,
                        RoomNumber = floor * 100 + roomOnFloor,
                        HotelId = h + 1,
                        RoomtypeId = (r % 6) + 1,
                        CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                        UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                    });
                }
            }
            modelBuilder.Entity<Room>().HasData(rooms);
        }

        private void SeedRoles(ModelBuilder modelBuilder)
        {
            var roles = new[]
            {
                new Role
                {
                    Id = 1,
                    Name = "User",
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Role
                {
                    Id = 2,
                    Name = "CleaningStaff",
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Role
                {
                    Id = 3,
                    Name = "Reception",
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Role
                {
                    Id = 4,
                    Name = "Admin",
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                }
            };
            modelBuilder.Entity<Role>().HasData(roles);
        }

        private void SeedUser(ModelBuilder modelBuilder)
        {
            var users = new[]
            {
                new User
                {
                    Id = 1,
                    FirstName = "test",
                    LastName = "test",
                    Email = "test@test.com",
                    Phone = 12345678,
                    HashedPassword = "$2a$11$BJtEDbA0yeNpnSNKPeGh7eCmVA6tIUoC.QLBFqMjGh.7MWUSGtKJe",
                    PasswordBackdoor = "!MyVerySecureSecretKeyThatIsAtLeast32CharactersLong123456789",
                    RoleId = 4,
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
            };
            modelBuilder.Entity<User>().HasData(users);
        }

        private void SeedRoomtype(ModelBuilder modelBuilder)
        {
            var roomtypes = new[]
            {
                new Roomtype
                {
                    Id = 1,
                    Name = "Enkeltværelse",
                    Description = "Et enkeltværelse med én seng, ideelt til én person.",
                    PricePerNight = 2999.99,
                    NumberOfBeds = 2,
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Roomtype
                {
                    Id = 2,
                    Name = "Dobbeltværelse",
                    Description = "Et dobbeltværelse med to senge eller en dobbeltseng.",
                    PricePerNight = 3299.99,
                    NumberOfBeds = 4,
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Roomtype
                {
                    Id = 3,
                    Name = "Suite",
                    Description = "En suite med ekstra plads og komfort, ofte med separat opholdsområde.",
                    PricePerNight = 3399.99,
                    NumberOfBeds = 7,
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Roomtype
                {
                    Id = 4,
                    Name = "Familieværelse",
                    Description = "Et værelse med plads til hele familien, typisk med flere senge.",
                    PricePerNight = 3499.99,
                    NumberOfBeds = 8,
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Roomtype
                {
                    Id = 5,
                    Name = "Deluxe værelse",
                    Description = "Et deluxe værelse med ekstra faciliteter og komfort.",
                    PricePerNight = 3599.99,
                    NumberOfBeds = 10,
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Roomtype
                {
                    Id = 6,
                    Name = "Handicapvenligt værelse",
                    Description = "Et værelse designet til gæster med særlige behov og nem adgang.",
                    PricePerNight = 3199.99,
                    NumberOfBeds = 5,
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                }
            };
            modelBuilder.Entity<Roomtype>().HasData(roomtypes);
        }

        private void SeedHotel(ModelBuilder modelBuilder)
        {
            var hotels = new[]
            {
                new Hotel
                {
                    Id = 1,
                    Name = "Hotel 1",
                    Road = "H. C. Andersens Vej 9",
                    Zip = "8800",
                    City = "Viborg",
                    Country = "Danmark",
                    Phone = 12345678,
                    Email = "mercantec@mercantec.dk",
                    Description = "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.",
                    PercentagePrice = 0,
                    OpenedAt = new TimeOnly(9, 0, 0),
                    ClosedAt = new TimeOnly(21, 30, 0),
                    CheckInFrom = new TimeOnly(16, 0, 0),
                    CheckInUntil = new TimeOnly(21, 0, 0),
                    CheckOutUntil = new TimeOnly(10, 0, 0),
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Hotel 2",
                    Road = "H. C. Andersens Vej 9",
                    Zip = "8800",
                    City = "Viborg",
                    Country = "Danmark",
                    Phone = 12345678,
                    Email = "mercantec@mercantec.dk",
                    Description = "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.",
                    PercentagePrice = 0.1,
                    OpenedAt = new TimeOnly(9, 0, 0),
                    ClosedAt = new TimeOnly(21, 30, 0),
                    CheckInFrom = new TimeOnly(16, 0, 0),
                    CheckInUntil = new TimeOnly(21, 0, 0),
                    CheckOutUntil = new TimeOnly(10, 0, 0),
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Hotel 3",
                    Road = "H. C. Andersens Vej 9",
                    Zip = "8800",
                    City = "Viborg",
                    Country = "Danmark",
                    Phone = 12345678,
                    Email = "mercantec@mercantec.dk",
                    Description = "First Central Hotel Suites er udstyret med 524 moderne suiter, der kan prale af moderne finish og en lokkende hyggelig stemning, der giver hver gæst den ultimative komfort og pusterum. Hotellet tilbyder en bred vifte af fritids- og forretningsfaciliteter, herunder et mini-businesscenter, rejseskrivebord, en fredfyldt pool på taget, veludstyret fitnesscenter og rekreative faciliteter.\r\nFra spisning til roomservice, oplev en balance mellem kontinentale retter og tilfredsstil dine trang med den friske gane i Beastro Restaurant og den søde duft af kaffe på Beastro, der ligger i lobbyen.",
                    PercentagePrice = 0.2,
                    OpenedAt = new TimeOnly(9, 0, 0),
                    ClosedAt = new TimeOnly(21, 30, 0),
                    CheckInFrom = new TimeOnly(16, 0, 0),
                    CheckInUntil = new TimeOnly(21, 0, 0),
                    CheckOutUntil = new TimeOnly(10, 0, 0),
                    CreatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2025, 1, 1, 10, 0, 0, DateTimeKind.Utc)
                },
            };
            modelBuilder.Entity<Hotel>().HasData(hotels);
        }
    }
}