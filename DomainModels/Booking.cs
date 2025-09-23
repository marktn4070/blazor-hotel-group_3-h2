using System.ComponentModel.DataAnnotations;
namespace DomainModels;

// Booking.cs
public class Booking : Common
{
    public int UserId { get; set; }
    public User? User { get; set; }

    public int RoomId { get; set; }
    public Room? Room { get; set; }
    public double? FinalPrice { get; set; }
    public BookingStatus BookingStatus { get; set; } = BookingStatus.Confirmed;
    public bool Crib { get; set; }
    public int ExtraBeds { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
public class BookingGetDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string? UserEmail { get; set; }
    public string? UserFirstName { get; set; }
    public string? UserLastName { get; set; }
    public int RoomId { get; set; }
    public int? RoomRoomNumber { get; set; }
    public double? RoomtypePricePerNight { get; set; }
    public string? HotelName { get; set; }
    public double? FinalPrice { get; set; }
    public BookingStatus BookingStatus { get; set; }
    public bool Crib { get; set; }
    public int ExtraBeds { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class BookingPostDto
{
    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Room ID is required")]
    public int RoomId { get; set; }
    public double? FinalPrice { get; set; }

    [Required(ErrorMessage = "Crib is required")]
    public bool Crib { get; set; }

    [Range(0, 2, ErrorMessage = "Extra beds must be between 0 and 2")]
    [Required(ErrorMessage = "Extra beds is required")]
    public int ExtraBeds { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }
}
public class BookingPutDto
{
    [Required(ErrorMessage = "Booking ID is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "User ID is required")]
    public int UserId { get; set; }

    [Required(ErrorMessage = "Room ID is required")]
    public int RoomId { get; set; }
    public double? FinalPrice { get; set; }
    public BookingStatus BookingStatus { get; set; } = BookingStatus.Confirmed;

    [Required(ErrorMessage = "Crib is required")]
    public bool Crib { get; set; }

    [Range(0, 2, ErrorMessage = "Extra beds must be between 0 and 2")]
    [Required(ErrorMessage = "Extra beds is required")]
    public int ExtraBeds { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    [Required(ErrorMessage = "End date is required")]
    public DateTime EndDate { get; set; }
}

public enum BookingStatus
{
    Confirmed,   // 0
    Cancelled,   // 1
    CheckedIn,   // 2
    CheckedOut   // 3
}
