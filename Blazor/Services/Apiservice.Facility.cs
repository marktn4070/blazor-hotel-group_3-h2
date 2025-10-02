using DomainModels;
using System.Net.Http;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public partial class APIService
    {
        public async Task<FacilityGetDto?> GetFacilityByIdAsync(int hotelId)
        {
            return await _httpClient.GetFromJsonAsync<FacilityGetDto>($"api/hotels/facility/{hotelId}");
        }
    }
}