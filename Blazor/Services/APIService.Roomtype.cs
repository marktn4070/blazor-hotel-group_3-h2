using System.Net.Http.Json;
using DomainModels;

namespace Blazor.Services;

public partial class APIService
{

    public async Task<RoomtypeGetDto[]> GetRoomtypesAsync(
        int maxItems,
        CancellationToken cancellationToken = default
    )
    {
        List<RoomtypeGetDto>? roomtypes = null;

        await foreach (
            var roomtype in _httpClient.GetFromJsonAsAsyncEnumerable<RoomtypeGetDto>(
                "/api/Roomtypes",
                cancellationToken
            )
        )
        {
            if (roomtypes?.Count >= maxItems && maxItems != 0)
            {
                break;
            }
            if (roomtype is not null)
            {
                roomtypes ??= [];
                roomtypes.Add(roomtype);
            }
        }

        return roomtypes?.ToArray() ?? [];
    }
    public async Task CreateRoomtypeAsync(RoomtypePostDto roomtype)
    {
        var response = await _httpClient.PostAsJsonAsync("api/roomtypes", roomtype);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"API error: {error}");
        }
    }
    public async Task UpdateRoomtypeAsync(RoomtypePutDto roomtype)
    {
        // Example implementation using HttpClient (adjust endpoint and logic as needed)
        var response = await _httpClient.PutAsJsonAsync($"api/roomtypes/{roomtype.Id}", roomtype);
        response.EnsureSuccessStatusCode();
    }
}
