using DomainModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public partial class APIService
    {
        public async Task<List<RoomGetDto>> GetRoomsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RoomGetDto>>("api/rooms");
        }

        public async Task<BookingGetDto> CreateBookingAsync(BookingPostDto booking/*,string fullToken = null*/)
        {
            //AuthenticationHeaderValue? original = _httpClient.DefaultRequestHeaders.Authorization;
            //if (!string.IsNullOrWhiteSpace(fullToken))
            //{
            //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", fullToken);
            //}
            var response = await _httpClient.PostAsJsonAsync("api/bookings", booking);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<BookingGetDto>();
        }

    }
}
