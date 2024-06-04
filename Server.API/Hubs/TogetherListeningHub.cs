using System.Text.Json;
using Joint.Data.Models;
using Microsoft.AspNetCore.SignalR;
using Server.API.Models;
using Server.BLL.Interfaces;
using Server.DAL.Interfaces.Repositories;

namespace Server.API.Hubs;

class RoomUser
{
    public int UserId;
    public User User;
    public string ConnectionId;
    public string CodeRoom;
}

public class TogetherListeningHub : Hub
{


    private readonly IUserService _userService ;
    private static readonly List<RoomUser> _roomUsers = new List<RoomUser>();
    private readonly Random _random = new Random();

    public TogetherListeningHub(IUserService userService)
    {
        _userService = userService;
    }

    private const string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("CONTECTED NEW CLIENT!");
        
        return base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var roomUser = _roomUsers.FirstOrDefault(i => i.ConnectionId == Context.ConnectionId);
        var jsonUser = JsonSerializer.Serialize(roomUser.User,
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        await Clients.OthersInGroup(roomUser.CodeRoom).SendAsync("USER_DISCONNECTED", jsonUser);
        _roomUsers.Remove(_roomUsers.FirstOrDefault(i => i.ConnectionId == Context.ConnectionId));
        Console.WriteLine($"DISCONECTED CLIENT! {exception}");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task<string> CreateRoom(string name)
    {
        string code;
        do
        {
            code = GenerateCode();
            
        } while (_roomUsers.Any(i => i.CodeRoom == code));

        _roomUsers.Add(new RoomUser
        {
            UserId = 0,
            User = _userService.ReadByEmail(name),
            ConnectionId = Context.ConnectionId,
            CodeRoom = code
        });
        await Groups.AddToGroupAsync(Context.ConnectionId, code);
        return code;
    } 
    
    public async Task<string> JoinRoom(string code, string name)
    {
        if (_roomUsers.FirstOrDefault(i => i.CodeRoom == code) == null)
        {
            return "ERROR;Не вдалося знайти таку кімнату";
        }
        
        
        var user =  _userService.ReadByEmail(name);
        Console.WriteLine($"JOIN ROOM WITH CODE {code} AND NAME {name}, CLINET ID IS {Context.ConnectionId}!");
        _roomUsers.Add(new RoomUser
        {
            UserId = 0,
            User = user,
            ConnectionId = Context.ConnectionId,
            CodeRoom = code
        });
        await Groups.AddToGroupAsync(Context.ConnectionId, code);
        await Clients.OthersInGroup(code).SendAsync("NEW_JOINED", JsonSerializer.Serialize(user, new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        }));
        return JsonSerializer.Serialize(_roomUsers.Select(i => i.User).Where(i => i.Email != name), new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }

    public async Task<string> KickUser(string group, int userId)
    {
        await Clients.Client(_roomUsers.FirstOrDefault(i => i.UserId == userId).ConnectionId).SendAsync("DISCONNECTED");

        var jsonUser = JsonSerializer.Serialize(_roomUsers.FirstOrDefault(i => i.UserId == userId).User,
            new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        
        await Clients.OthersInGroup(group).SendAsync("USER_DISCONNECTED", jsonUser);
        return "OK";
    }    

    public async Task<string> StartPlayingTrack(string group, string jsonTrack)
    {
        Console.WriteLine($"START PLAY ON GROUP {group}\n" +
                          $"AND JSON TRACK {jsonTrack}, CLINET ID IS {Context.ConnectionId}!");
        await Clients.OthersInGroup(group).SendAsync("START_PLAY", jsonTrack);
        return "OK";
    }
    
    public async Task<String> StopPlayingTrack(string group)
    {
        await Clients.OthersInGroup(group).SendAsync("PAUSE_PLAY");
        return "OK";
    }
    
    public async Task<String> ResumePlayingTrack(string group)
    {
        await Clients.OthersInGroup(group).SendAsync("RESUME_PLAY");
        return "OK";
    }

    private string GenerateCode(int length = 6)
    {
        return new string(Enumerable.Repeat(Chars, length)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
    
}