using DomainModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace Blazor.Services
{
    public partial class APIService
    {
        // User authentication methods
        public async Task<LoginApiResult> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new LoginApiResult
                    {
                        Success = true,
                        Response = loginResponse,
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(errorContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new LoginApiResult
                    {
                        Success = false,
                        ErrorResponse = errorResponse,
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved login: " + ex.Message);
                return new LoginApiResult
                {
                    Success = false,
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }

        //public async Task<bool> RegisterAsync(RegisterDto registerDto)
        //{
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync("api/users/register", registerDto);
        //        return response.IsSuccessStatusCode;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Fejl ved registrering: " + ex.Message);
        //        return false;
        //    }
        //}


        public async Task<RegisterApiResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/users/register", registerDto);
                var content = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    // Hvis API'en returnerer en besked kan du deserialisere den her (valgfrit)
                    return new RegisterApiResult
                    {
                        Success = true,
                        Message = "Konto oprettet succesfuldt.",
                        StatusCode = response.StatusCode
                    };
                }
                else
                {
                    ErrorResponse? err = null;
                    try
                    {
                        err = JsonSerializer.Deserialize<ErrorResponse>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                    }
                    catch { /* ignore parse errors */ }

                    return new RegisterApiResult
                    {
                        Success = false,
                        Message = err?.Message ?? content,
                        ErrorResponse = err,
                        StatusCode = response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved registrering: {ex.Message}");
                return new RegisterApiResult
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }

        public async Task<UserGetDto[]?> GetUsersAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<UserGetDto[]>("api/users");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved hentning af brugere: " + ex.Message);
                return null;
            }
        }

        public async Task<UserGetDto?> GetUserAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<UserGetDto>($"api/users/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved hentning af bruger {id}: " + ex.Message);
                return null;
            }
        }

        public async Task<UserGetDto?> GetCurrentUserAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<UserGetDto>("api/users/me");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Fejl ved hentning af nuværende bruger: " + ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/users/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fejl ved sletning af bruger {id}: " + ex.Message);
                return false;
            }
        }

        // Logout method (client-side token removal)
        public void Logout()
        {
            // This would typically clear the JWT token from storage
            // Implementation depends on how you store the token
        }
    }

    // Response models for API calls
    public class LoginApiResult
    {
        public bool Success { get; set; }
        public LoginResponse? Response { get; set; }
        public ErrorResponse? ErrorResponse { get; set; }
        public string? ErrorMessage { get; set; }
        public System.Net.HttpStatusCode StatusCode { get; set; }
    }

    public class RegisterApiResult
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public ErrorResponse? ErrorResponse { get; set; }
        public System.Net.HttpStatusCode StatusCode { get; set; }
    }



    public class LoginResponse
    {
        public string Message { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public AuthUserInfo User { get; set; } = new();
    }

    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public int RemainingLockoutSeconds { get; set; }
        public int DelayApplied { get; set; }
    }

    public class AuthUserInfo
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public AuthUserInfo? User { get; set; }
        public string? Token { get; set; }
        public int RemainingLockoutSeconds { get; set; }
        public int DelayApplied { get; set; }
    }
}
