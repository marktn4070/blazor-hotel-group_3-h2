using Blazor.Pages;
using DomainModels;
using System.Net.Http.Json;

namespace Blazor.Services;

public partial class APIService
{

    public async Task<RoomGetDto[]> GetRoomsAsync(
        int maxItems,
        CancellationToken cancellationToken = default
    )
    {
        List<RoomGetDto>? rooms = null;

        await foreach (
            var room in _httpClient.GetFromJsonAsAsyncEnumerable<RoomGetDto>(
                "/api/Rooms",
                cancellationToken
            )
        )
        {
            if (rooms?.Count >= maxItems && maxItems != 0)
            {
                break;
            }
            if (room is not null)
            {
                rooms ??= [];
                rooms.Add(room);
            }
        }

        return rooms?.ToArray() ?? [];
    }


    public async Task<RoomGetDto[]> GetRoomAsync(
        int roomId,
        CancellationToken cancellationToken = default
    )
    {
        RoomGetDto? room = null;

        if (roomId != 0)
        {
            try
            {
                room = await _httpClient.GetFromJsonAsync<RoomGetDto>($"/api/Rooms/{roomId}", cancellationToken);
            }
            catch (HttpRequestException)
            {
                return Array.Empty<RoomGetDto>();
            }
        }

        return room is null ? Array.Empty<RoomGetDto>() : new[] { room };
    }
    public async Task<RoomGetDto?> GetRoom(
        int roomId,
        CancellationToken cancellationToken = default
    )
    {
        if (roomId == 0)
            return null;

        try
        {
            return await _httpClient.GetFromJsonAsync<RoomGetDto>(
                $"/api/Rooms/{roomId}",
                cancellationToken
            );
        }
        catch (HttpRequestException)
        {
            return null;
        }
    }

    
    //wip
    public async Task CreateRoomAsync(RoomPostDto room)
	{
		var response = await _httpClient.PostAsJsonAsync("api/rooms", room);

		if (!response.IsSuccessStatusCode)
		{
			var error = await response.Content.ReadAsStringAsync();
			throw new Exception($"API error: {error}");
		}
	}
	public async Task UpdateRoomAsync(RoomPutDto room)
	{
		// Example implementation using HttpClient (adjust endpoint and logic as needed)
		var response = await _httpClient.PutAsJsonAsync($"api/rooms/{room.Id}", room);
		response.EnsureSuccessStatusCode();
	}
    public async Task DeleteRoom(int Id)
    {
        var response = await _httpClient.DeleteAsync($"api/Rooms/{Id}");
        response.EnsureSuccessStatusCode();
    }
}
