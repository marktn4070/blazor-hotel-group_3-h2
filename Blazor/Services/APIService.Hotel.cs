using System.Net.Http.Json;
using DomainModels;

namespace Blazor.Services;

public partial class APIService
{

    public async Task<HotelGetDto[]> GetHotelsAsync(
        int maxItems,
        CancellationToken cancellationToken = default
    )
    {
        List<HotelGetDto>? hotels = null;

        await foreach (
            var hotel in _httpClient.GetFromJsonAsAsyncEnumerable<HotelGetDto>(
                "/api/Hotels",
                cancellationToken
            )
        )
        {
            if (hotels?.Count >= maxItems && maxItems != 0)
            {
                break;
            }
            if (hotel is not null)
            {
                hotels ??= [];
                hotels.Add(hotel);
            }
        }

        return hotels?.ToArray() ?? [];
    }
    public async Task CreateHotelAsync(HotelPostDto hotel)
    {
        var response = await _httpClient.PostAsJsonAsync("api/hotels", hotel);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API error: {error}");
        }
    }
    public async Task UpdateHotelAsync(HotelPutDto hotel)
    {
        // Example implementation using HttpClient (adjust endpoint and logic as needed)
        var response = await _httpClient.PutAsJsonAsync($"api/hotels/{hotel.Id}", hotel);
        response.EnsureSuccessStatusCode();
    }

    public async Task<HotelGetDto?> GetHotelByIdAsync(
        int hotelId,
        CancellationToken cancellationToken = default
    )
    {
        HotelGetDto? hotel = null;

        if (hotelId != 0)
        {
            try
            {
                hotel = await _httpClient.GetFromJsonAsync<HotelGetDto>($"api/hotels/{hotelId}", cancellationToken);
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }
        return hotel;
    }


    public async Task DeleteHotelAsync(
        int hotelId,
        CancellationToken cancellationToken = default
    )
    {
        var response = null as HttpResponseMessage;

        if (hotelId != 0)
        {
            try
            {
                response = await _httpClient.DeleteAsync($"api/hotels/{hotelId}", cancellationToken);
            }
            catch (HttpRequestException)
            {
                return;
            }
        }
        response.EnsureSuccessStatusCode();
    }


}
