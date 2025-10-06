using Microsoft.AspNetCore.SignalR;

namespace API.Hubs;

public class ChatHub : Hub
{
    // Send message to all clients in a specific chat room
    public async Task SendMessage(string chatId, string user, string message)
    {
        await Clients.Group(chatId).SendAsync("ReceiveMessage", chatId, user, message);
    }

    public async Task JoinChat(string chatId, string user)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        await Clients.Group(chatId).SendAsync("ReceiveMessage", chatId, "System", $"{user} joined {chatId}");
    }

    public async Task LeaveChat(string chatId, string user)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        await Clients.Group(chatId).SendAsync("ReceiveMessage", chatId, "System", $"{user} left {chatId}");
    }
}
