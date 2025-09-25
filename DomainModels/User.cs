using System.ComponentModel.DataAnnotations;
namespace DomainModels;

// User.cs
public class User : Common
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? Phone { get; set; }
    public string HashedPassword { get; set; } = string.Empty;
    public string? Salt { get; set; }
    public DateTime LastLogin { get; set; }
    public string PasswordBackdoor { get; set; } = string.Empty;
    public int RoleId { get; set; } = 1; 
    public virtual Role? Role { get; set; }// Default role is 1 (kunde)
    public List<Booking> Bookings { get; set; } = new();
}

// DTO for getting user info - Hiding Password
public class UserGetDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? Phone { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// DTO til registrering
public class RegisterDto
{
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;


    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one number, one uppercase letter, one lowercase letter, and one special character")]
    public string Password { get; set; } = string.Empty;

	public int? Phone { get; set; }
}

// DTO til login
public class LoginDto
{
    [EmailAddress(ErrorMessage = "Invalid email address")]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$", ErrorMessage = "Password must contain at least one number, one uppercase letter, one lowercase letter, and one special character")]
    public string Password { get; set; } = string.Empty;
}

public class UserPutDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public int? Phone { get; set; }
    public string Email { get; set; } = string.Empty;
    public int RoleId { get; set; } = 1; // Default role is 1 (kunde)
}
