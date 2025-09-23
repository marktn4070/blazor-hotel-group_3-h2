using System.ComponentModel.DataAnnotations;
namespace DomainModels;

// Role.cs

/// <summary>
/// Role entity til database
/// </summary>
public class Role : Common
{
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Navigation property til brugere med denne rolle
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}