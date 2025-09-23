using System.ComponentModel.DataAnnotations;
namespace DomainModels;

// Facility.cs
public class Facility : Common
{
    public bool Pool { get; set; }
    public bool Fitness { get; set; }
    public bool Restaurant { get; set; }
    public int HotelId { get; set; }
    public Hotel? Hotel { get; set; }
}
public class FacilityGetDto
{
    public int Id { get; set; }
    public bool Pool { get; set; }
    public bool Fitness { get; set; }
    public bool Restaurant { get; set; }
    public int HotelId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
public class FacilityPostDto
{
    public bool Pool { get; set; }
    public bool Fitness { get; set; }
    public bool Restaurant { get; set; }

    [Required(ErrorMessage = "Hotel ID is required")]
    public int HotelId { get; set; }
}
public class FacilityPutDto
{
    [Required(ErrorMessage = "Facility ID is required")]
    public int Id { get; set; }
    public bool Pool { get; set; }
    public bool Fitness { get; set; }
    public bool Restaurant { get; set; }

    [Required(ErrorMessage = "Hotel ID is required")]
    public int HotelId { get; set; }
}
