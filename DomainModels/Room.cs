using System.ComponentModel.DataAnnotations;
namespace DomainModels;

// Room.cs
public class Room : Common
{
    public int RoomNumber { get; set; }
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
    public int RoomtypeId { get; set; }
    public Roomtype? Roomtype { get; set; }
    public List<Booking> Bookings { get; set; } = new();
}
public class RoomGetDto
{
    public int Id { get; set; }
    public int RoomNumber { get; set; }
    public int HotelId { get; set; }
    public int RoomtypeId { get; set; }
    public double? RoomtypePricePerNight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// DTO for room creation / POST
public class RoomPostDto
{
    [Required(ErrorMessage = "Room number is required")]
    public int RoomNumber { get; set; }

    [Required(ErrorMessage = "Hotel ID is required")]
    public int HotelId { get; set; }

    [Required(ErrorMessage = "Room type ID is required")]
    public int RoomtypeId { get; set; }
}

// DTO for room update / PUT
public class RoomPutDto
{
    [Required(ErrorMessage = "Room ID is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Room number is required")]
    public int RoomNumber { get; set; }

    [Required(ErrorMessage = "Hotel ID is required")]
    public int HotelId { get; set; }

    [Required(ErrorMessage = "Room type ID is required")]
    public int RoomtypeId { get; set; }
}
