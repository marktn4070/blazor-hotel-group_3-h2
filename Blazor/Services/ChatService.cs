using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

public class ChatService
{
    private readonly NavigationManager _navigation;
    private HubConnection? _hubConnection;

    public event Action<string, string, string>? OnMessageReceived;
    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public ChatService(NavigationManager navigation)
    {
        _navigation = navigation;
    }

    public async Task ConnectAsync()
    {
        if (_hubConnection != null && IsConnected) return;

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigation.ToAbsoluteUri("https://localhost:8091/chathub"))
            .WithAutomaticReconnect()
            .Build();

        _hubConnection.On<string, string, string>("ReceiveMessage", (chatId, user, message) =>
        {
            OnMessageReceived?.Invoke(chatId, user, message);
        });

        await _hubConnection.StartAsync();
    }

    public async Task SendMessage(string chatId, string user, string message)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.SendAsync("SendMessage", chatId, user, message);
        }
    }

    public async Task JoinChat(string chatId, string user)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.SendAsync("JoinChat", chatId, user);
        }
    }

    public async Task LeaveChat(string chatId, string user)
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.SendAsync("LeaveChat", chatId, user);
        }
    }
}
