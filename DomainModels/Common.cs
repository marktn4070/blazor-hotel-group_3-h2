using System.ComponentModel.DataAnnotations;
namespace DomainModels;

// Common.cs
public class Common
{
    [Key]
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
