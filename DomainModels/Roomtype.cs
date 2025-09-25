using System.ComponentModel.DataAnnotations;
namespace DomainModels;

// Roomtype.cs
public class Roomtype : Common
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NumberOfBeds { get; set; }
    public double PricePerNight { get; set; }
    public List<Room> Rooms { get; set; } = new(); // 1:n

}
public class RoomtypeGetDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NumberOfBeds { get; set; }
    public double PricePerNight { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

}
public class RoomtypePostDto
{

    [Required(ErrorMessage = "The name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The description is required")]
    [StringLength(200, ErrorMessage = "The description must be a maximum of 200 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "The amount of beds is required")]
    [Range(1, 10, ErrorMessage = "The amount of beds must be between 1 og 10")]
    public int NumberOfBeds { get; set; }

    [Required(ErrorMessage = "The price per night is required")]
    public double PricePerNight { get; set; }

}
public class RoomtypePutDto
{

    [Required(ErrorMessage = "The room type ID is required")]
    public int Id { get; set; }

    [Required(ErrorMessage = "The name is required")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The description is required")]
    [StringLength(600, ErrorMessage = "The description must be a maximum of 600 characters")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "The amount of beds is required")]
    [Range(1, 10, ErrorMessage = "The amount of beds must be between 1 og 10")]
    public int NumberOfBeds { get; set; }

    [Required(ErrorMessage = "The price per night is required")]
    public double PricePerNight { get; set; }
}
public class RoomtypeDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int NumberOfBeds { get; set; }
    public double PricePerNight { get; set; }

}
