using DomainModels;
using System.Net.Http.Json;

namespace Blazor.Services
{
    public partial class APIService
    {
        public async Task<HealthCheckResponse?> GetHealthCheckAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<HealthCheckResponse>("api/status/healthcheck");
            }
            catch (Exception ex)
            {
                // Her kan du evt. logge fejlen
                Console.WriteLine("Fejl ved HealthCheck: " + ex.Message);
                return new HealthCheckResponse
                {
                    status = "Error",
                    message = "Kunne ikke hente API-status (" + ex.Message + ")"
                };
            }
        }
        public async Task<HealthCheckResponse?> GetDBHealthCheckAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<HealthCheckResponse>("api/status/dbhealthcheck");
            }
            catch (Exception ex)
            {
                // Her kan du evt. logge fejlen
                Console.WriteLine("Fejl ved DBHealthCheck: " + ex.Message);
                return new HealthCheckResponse
                {
                    status = "Error",
                    message = "Kunne ikke hente database-status (" + ex.Message + ")"
                };
            }
        }
    }
}