using System.Numerics;

namespace DomainModels.Mapping;

// HotelMapping.cs
public class UserMapping
{
    public static UserGetDto ToUserGetDto(User user)
    {
        return new UserGetDto
        {
            Id = user.Id,
            Email = user.Email,
            Phone = user.Phone,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Role = user.Role?.Name ?? string.Empty,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }

    public static List<UserGetDto> ToUsersGetDto(List<User> users)
    {
        return users.Select(u => ToUserGetDto(u)).ToList();
    }
    public static void PutUserFromDto(User user, UserPutDto userPutDto)
    {
        user.Id = userPutDto.Id;
            user.Email = userPutDto.Email;
            user.Phone = userPutDto.Phone;
            user.FirstName = userPutDto.FirstName;
            user.LastName = userPutDto.LastName;
            user.UpdatedAt = DateTime.UtcNow.AddHours(2);
    }
}
