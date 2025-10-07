using DomainModels;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blazor.Services;

public partial class APIService
{


    // /api/bookings endpoint get all bookings
    public async Task<BookingGetDto[]?> GetAllBookingsAsync(
        int maxItems,
        string? fullToken = null,
        CancellationToken cancellationToken = default
    )
    {
        AuthenticationHeaderValue? original = _httpClient.DefaultRequestHeaders.Authorization;
        try
        {
            if (!string.IsNullOrWhiteSpace(fullToken))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fullToken);
            }

            List<BookingGetDto>? bookings = null;

            await foreach (
                var booking in _httpClient.GetFromJsonAsAsyncEnumerable<BookingGetDto>(
                    "/api/Bookings",
                    cancellationToken
                )
            )
            {
                if (bookings?.Count >= maxItems && maxItems != 0)
                {
                    break;
                }
                if (booking is not null)
                {
                    bookings ??= [];
                    bookings.Add(booking);
                }
            }

            return bookings?.ToArray() ?? [];
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fejl ved hentning af bookninge: " + ex.Message);
            return null;
        }
            finally
        {
            // Gendan tidligere header (eller fjern hvis der ikke var nogen)
            _httpClient.DefaultRequestHeaders.Authorization = original;
        }
    }


    // /me endpoint get current booking
    public async Task<BookingGetDto?> GetBookingAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<BookingGetDto>($"api/bookings/{id}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved hentning af bookning {id}: " + ex.Message);
            return null;
        }
    }
    public async Task<BookingGetDto[]> GetAllBookingsAsync(
    int maxItems,
    CancellationToken cancellationToken = default
)
    {
        List<BookingGetDto>? bookings = null;

        await foreach (
            var booking in _httpClient.GetFromJsonAsAsyncEnumerable<BookingGetDto>(
                "/api/Bookings",
                cancellationToken
            )
        )
        {
            if (bookings?.Count >= maxItems && maxItems != 0)
            {
                break;
            }
            if (booking is not null)
            {
                bookings ??= [];
                bookings.Add(booking);
            }
        }

        return bookings?.ToArray() ?? [];
    }

    public async Task<BookingGetDto?> GetCurrentBookingAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<BookingGetDto>("api/bookings/me");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Fejl ved hentning af nuværende bookning: " + ex.Message);
            return null;
        }
    }


    public async Task<BookingGetDto> CreateBookingAsync(BookingPostDto booking)
    {
        // Make sure the dates are UTC before sending
        booking.StartDate = DateTime.SpecifyKind(booking.StartDate, DateTimeKind.Utc);
        booking.EndDate = DateTime.SpecifyKind(booking.EndDate, DateTimeKind.Utc);

        var response = await _httpClient.PostAsJsonAsync("api/bookings", booking);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<BookingGetDto>()
            ?? throw new Exception("Server returned invalid response");
    }
    public async Task<bool> DeleteBookingAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/bookings/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved sletning af bookning {id}: " + ex.Message);
            return false;
        }
    }
}
