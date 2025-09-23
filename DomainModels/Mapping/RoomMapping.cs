namespace DomainModels.Mapping;

// RoomMapping.cs
public class RoomMapping
    {
public static RoomGetDto ToRoomGetDto(Room room)
{
    return new RoomGetDto
    {
        Id = room.Id,
        RoomNumber = room.RoomNumber,
        HotelId = room.HotelId,
        RoomtypeId = room.RoomtypeId,
        CreatedAt = room.CreatedAt,
        UpdatedAt = room.UpdatedAt
    };
}

public static List<RoomGetDto> ToRoomGetDtos(List<Room> rooms)
{
    return rooms.Select(r => ToRoomGetDto(r)).ToList();
}

public static Room PostRoomFromDto(RoomPostDto roomPostDto)
{
    return new Room
    {
        RoomNumber = roomPostDto.RoomNumber,
        HotelId = roomPostDto.HotelId,
        RoomtypeId = roomPostDto.RoomtypeId,
        CreatedAt = DateTime.UtcNow.AddHours(2),
        UpdatedAt = DateTime.UtcNow.AddHours(2)
    };
}

public static void PutRoomFromDto(Room room, RoomPutDto roomPutDto)
{
    room.Id = roomPutDto.Id;
    room.RoomNumber = roomPutDto.RoomNumber;
    room.HotelId = roomPutDto.HotelId;
    room.RoomtypeId = roomPutDto.RoomtypeId;
    room.UpdatedAt = DateTime.UtcNow.AddHours(2);
}
}
