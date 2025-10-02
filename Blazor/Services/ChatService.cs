using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Blazor.Services
{
    public class ChatService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationService _authService;
        private HubConnection? _hubConnection;

    public event Action<string, string, string>? OnMessageReceived;
        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        // Events for components to subscribe to
        public event Action<string, string, string>? OnMessageReceived;
        public event Action? OnConnected;
        public event Action? OnDisconnected;

        public ChatService(HttpClient httpClient, AuthenticationService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private string GetHubUrl()
        {
            // new Uri(base, "chathub") sikrer korrekt sammensætning uanset trailing slash
            return new Uri(_httpClient.BaseAddress!, "chathub").ToString();
        }

        public async Task ConnectAsync(CancellationToken cancellationToken = default)
        {
            if (_hubConnection != null && (_hubConnection.State == HubConnectionState.Connected || _hubConnection.State == HubConnectionState.Connecting))
                return;

            var token = await _authService.GetTokenAsync();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(GetHubUrl(), options =>
                {
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        options.AccessTokenProvider = () => Task.FromResult(token);
                    }
                })
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<string, string, string>("ReceiveMessage", (chatId, user, message) =>
            {
                OnMessageReceived?.Invoke(chatId, user, message);
            });

            await _hubConnection.StartAsync(cancellationToken);
            OnConnected?.Invoke();
        }

        public async Task SendMessage(string chatId, string user, string message)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await ConnectAsync();

            if (_hubConnection != null)
                await _hubConnection.SendAsync("SendMessage", chatId, user, message);
        }
    }

        public async Task JoinChat(string chatId, string user)
        {
            if (_hubConnection == null || _hubConnection.State != HubConnectionState.Connected)
                await ConnectAsync();

            if (_hubConnection != null)
                await _hubConnection.SendAsync("JoinChat", chatId, user);
        }
    }

        public async Task LeaveChat(string chatId, string user)
        {
            if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
            {
                await _hubConnection.SendAsync("LeaveChat", chatId, user);
            }
        }
    }
}
