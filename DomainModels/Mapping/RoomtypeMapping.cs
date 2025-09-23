namespace DomainModels.Mapping;

// RoomtypeMapping.cs
public class RoomtypeMapping
{
    public static RoomtypeGetDto ToRoomtypeGetDto(Roomtype roomtype)
    {
        return new RoomtypeGetDto
        {
            Id = roomtype.Id,
            Name = roomtype.Name,
            Description = roomtype.Description,
            NumberOfBeds = roomtype.NumberOfBeds,
            PricePerNight = roomtype.PricePerNight,
            CreatedAt = roomtype.CreatedAt,
            UpdatedAt = roomtype.UpdatedAt
        };
    }

    public static List<RoomtypeGetDto> ToRoomtypeGetDtos(List<Roomtype> roomtypes)
    {
        return roomtypes.Select(r => ToRoomtypeGetDto(r)).ToList();
    }

    public static Roomtype PostRoomtypeFromDto(RoomtypePostDto roomtypePostDto)
    {
        return new Roomtype
        {
            Name = roomtypePostDto.Name,
            Description = roomtypePostDto.Description,
            NumberOfBeds = roomtypePostDto.NumberOfBeds,
            PricePerNight = roomtypePostDto.PricePerNight,
            CreatedAt = DateTime.UtcNow.AddHours(2),
            UpdatedAt = DateTime.UtcNow.AddHours(2)
        };
    }

    public static void PutRoomtypeFromDto(Roomtype roomtype, RoomtypePutDto roomtypePutDto)
    {
        roomtype.Id = roomtypePutDto.Id;
        roomtype.Name = roomtypePutDto.Name;
        roomtype.Description = roomtypePutDto.Description;
        roomtype.NumberOfBeds = roomtypePutDto.NumberOfBeds;
        roomtype.PricePerNight = roomtypePutDto.PricePerNight;
        roomtype.UpdatedAt = DateTime.UtcNow.AddHours(2);
    }
}
