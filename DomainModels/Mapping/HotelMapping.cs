namespace DomainModels.Mapping;

// HotelMapping.cs
public class HotelMapping
{
    public static HotelGetDto ToHotelGetDto(Hotel hotel)
    {
        return new HotelGetDto
        {
            Id = hotel.Id,
            Name = hotel.Name,
            Road = hotel.Road,
            Zip = hotel.Zip,
            City = hotel.City,
            Country = hotel.Country,
            Phone = hotel.Phone,
            Email = hotel.Email,
            PercentagePrice = hotel.PercentagePrice,
            Description = hotel.Description,
            OpenedAt = hotel.OpenedAt,
            ClosedAt = hotel.ClosedAt,
            CheckInFrom = hotel.CheckInFrom,
            CheckInUntil = hotel.CheckInUntil,
            CheckOutUntil = hotel.CheckOutUntil
        };
    }

    public static List<HotelGetDto> ToHotelGetDtos(List<Hotel> hotels)
    {
        return hotels.Select(h => ToHotelGetDto(h)).ToList();
    }

    public static Hotel PostHotelFromDto(HotelPostDto hotelPostDto)
    {
        return new Hotel
        {
            Name = hotelPostDto.Name,
            Road = hotelPostDto.Road,
            Zip = hotelPostDto.Zip,
            City = hotelPostDto.City,
            Country = hotelPostDto.Country,
            Phone = hotelPostDto.Phone,
            Email = hotelPostDto.Email,
            PercentagePrice = hotelPostDto.PercentagePrice,
            Description = hotelPostDto.Description,
            OpenedAt = hotelPostDto.OpenedAt,
            ClosedAt = hotelPostDto.ClosedAt,
            CheckInFrom = hotelPostDto.CheckInFrom,
            CheckInUntil = hotelPostDto.CheckInUntil,
            CheckOutUntil = hotelPostDto.CheckOutUntil,
            CreatedAt = DateTime.UtcNow.AddHours(2),
            UpdatedAt = DateTime.UtcNow.AddHours(2)
        };
    }

    public static void PutHotelFromDto(Hotel hotel, HotelPutDto hotelPutDto)
    {
        hotel.Name = hotelPutDto.Name;
        hotel.Road = hotelPutDto.Road;
        hotel.Zip = hotelPutDto.Zip;
        hotel.City = hotelPutDto.City;
        hotel.Country = hotelPutDto.Country;
        hotel.Phone = hotelPutDto.Phone;
        hotel.Email = hotelPutDto.Email;
        hotel.PercentagePrice = hotelPutDto.PercentagePrice;
        hotel.Description = hotelPutDto.Description;
        hotel.OpenedAt = hotelPutDto.OpenedAt;
        hotel.ClosedAt = hotelPutDto.ClosedAt;
        hotel.CheckInFrom = hotelPutDto.CheckInFrom;
        hotel.CheckInUntil = hotelPutDto.CheckInUntil;
        hotel.CheckOutUntil = hotelPutDto.CheckOutUntil;
        hotel.UpdatedAt = DateTime.UtcNow.AddHours(2);
    }
}
