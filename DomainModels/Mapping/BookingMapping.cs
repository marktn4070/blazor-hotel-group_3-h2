namespace DomainModels.Mapping;

// BookingMapping.cs
public class BookingMapping
{
    /// <summary>
    /// Konverterer Booking til BookingGetDto
    /// </summary>
    public static BookingGetDto ToBookingGetDto(Booking booking)
    {
        return new BookingGetDto
        {
            Id = booking.Id,
            UserId = booking.UserId,
            UserEmail = booking.User?.Email,
            UserFirstName = booking.User?.FirstName,
            UserLastName = booking.User?.LastName,
            RoomId = booking.RoomId,
            RoomRoomNumber = booking.Room?.RoomNumber,
            RoomtypePricePerNight = booking.Room?.Roomtype?.PricePerNight,
            HotelName = booking.Room?.Hotel?.Name ?? string.Empty,
            StartDate = booking.StartDate,
            EndDate = booking.EndDate,
            FinalPrice = booking.FinalPrice,
            BookingStatus = booking.BookingStatus,
            Crib = booking.Crib,
            ExtraBeds = booking.ExtraBeds,
            CreatedAt = booking.CreatedAt,
            UpdatedAt = booking.UpdatedAt
        };
    }

    /// <summary>
    /// Konverterer liste af Booking til liste af BookingGetDto
    /// </summary>
    public static List<BookingGetDto> ToBookingGetDtos(List<Booking> bookings)
    {
        return bookings.Select(b => ToBookingGetDto(b)).ToList();
    }

    /// <summary>
    /// Konverterer BookingPostDto til Booking entity
    /// </summary>
    public static Booking ToBookingFromPostDto(BookingPostDto bookingPostDto, double roomPricePerNight)
    {
        var nights = (bookingPostDto.EndDate - bookingPostDto.StartDate).Days;
        return new Booking
        {
            UserId = bookingPostDto.UserId,
            RoomId = bookingPostDto.RoomId,
            StartDate = bookingPostDto.StartDate,
            EndDate = bookingPostDto.EndDate,
            FinalPrice = roomPricePerNight * nights,
            BookingStatus = 0,
            Crib = bookingPostDto.Crib,
            ExtraBeds = bookingPostDto.ExtraBeds,
            CreatedAt = DateTime.UtcNow.AddHours(2),
            UpdatedAt = DateTime.UtcNow.AddHours(2)
        };
    }

    /// <summary>
    /// Opdaterer Booking entity med data fra BookingPutDto
    /// </summary>
    public static void UpdateBookingFromPutDto(Booking booking, BookingPutDto bookingPutDto, double roomPricePerNight)
    {
        var nights = (bookingPutDto.EndDate - bookingPutDto.StartDate).Days;

        booking.UserId = bookingPutDto.UserId;
        booking.RoomId = bookingPutDto.RoomId;
        booking.StartDate = bookingPutDto.StartDate;
        booking.EndDate = bookingPutDto.EndDate;
        booking.FinalPrice = roomPricePerNight * nights;
        booking.BookingStatus = 0;
        booking.Crib = bookingPutDto.Crib;
        booking.ExtraBeds = bookingPutDto.ExtraBeds;
        booking.UpdatedAt = DateTime.UtcNow.AddHours(2);
    }
}
