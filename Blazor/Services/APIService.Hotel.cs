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
        var response = await _httpClient.PutAsJsonAsync($"api/hotels/{hotel.Id}", hotel);
        response.EnsureSuccessStatusCode();
    }
    public async Task<HotelDetailsDto?> GetHotelByIdAsync(int id)
    {
        return await _httpClient.GetFromJsonAsync<HotelDetailsDto>($"api/hotels/{id}");
    }

    public async Task DeleteHotelAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/hotels/{id}");
        response.EnsureSuccessStatusCode();
    }

}
