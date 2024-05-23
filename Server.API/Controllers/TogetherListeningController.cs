using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.BLL.Interfaces;
using Server.BLL.Services;

namespace Server.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class TogetherListeningController : ControllerBase
{
    private readonly IUserService _service;
    public TogetherListeningController(IUserService service)
    {
        _service = service;
    }
    
    [HttpGet("code")]
    public string GetCode()
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);

        return $"#{_service.ReadByEmail(claim.Value).Id}";
    }
}