using System.ComponentModel.DataAnnotations;
namespace DomainModels;

// Facility.cs
public class Facility
{
    [Key]
    public int HotelId { get; set; }
    public bool Pool { get; set; }
    public bool Fitness { get; set; }
    public bool Restaurant { get; set; }
    public Hotel? Hotel { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
public class FacilityGetDto
{
    public int HotelId { get; set; }
    public bool Pool { get; set; }
    public bool Fitness { get; set; }
    public bool Restaurant { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
public class FacilityPostDto
{
    [Required(ErrorMessage = "Hotel ID is required")]
    public int HotelId { get; set; }
    public bool Pool { get; set; }
    public bool Fitness { get; set; }
    public bool Restaurant { get; set; }
}
public class FacilityPutDto
{

    [Required(ErrorMessage = "Hotel ID is required")]
    public int HotelId { get; set; }
    public bool Pool { get; set; }
    public bool Fitness { get; set; }
    public bool Restaurant { get; set; }
}
