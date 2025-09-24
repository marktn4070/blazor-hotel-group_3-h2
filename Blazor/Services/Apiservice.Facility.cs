using DomainModels;
using System.Net.Http;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public partial class APIService
    {
        public async Task<FacilityDto?> GetFacilityByIdAsync(int hotelId)
        {
            return await _httpClient.GetFromJsonAsync<FacilityDto>($"api/hotels/facility/{hotelId}");
        }
    }
}