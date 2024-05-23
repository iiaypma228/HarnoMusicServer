using Microsoft.AspNetCore.SignalR;
using Server.API.Models;

namespace Server.API.Hubs;

public class TogetherListeningHub : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("CONTECTED NEW CLIENT!");
        
        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        Console.WriteLine($"DISCONECTED CLIENT! {exception}");
        return base.OnDisconnectedAsync(exception);
    }

    public void JoinRoom(string code, string name)
    {
        Console.WriteLine($"JOIN ROOM WITH CODE {code} AND NAME {name}, CLINET ID IS {Context.ConnectionId}!");
        Groups.AddToGroupAsync(Context.ConnectionId, code);
        Clients.OthersInGroup(code).SendAsync("NEW_JOINED", name);
    }

    public async void StartPlayingTrack(string group, string jsonTrack)
    {
        Console.WriteLine($"START PLAY ON GROUP {group}\n" +
                          $"AND JSON TRACK {jsonTrack}, CLINET ID IS {Context.ConnectionId}!");
        await Clients.OthersInGroup(group).SendAsync("START_PLAY", jsonTrack);
    }
    
    public async Task StopPlayingTrack(string group)
    {
        await Clients.OthersInGroup(group).SendAsync("PAUSE_PLAY");
    }
    
    public async Task ResumePlayingTrack(string group)
    {
        await Clients.OthersInGroup(group).SendAsync("RESUME_PLAY");
    }
}