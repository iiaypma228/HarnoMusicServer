using System.Security.Claims;
using Joint.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.BLL.Interfaces;

namespace Server.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class UserController : BaseCRUDController<User, IUserService>
{
    public UserController(IUserService service) : base(service)
    {
    }

    [HttpGet(Joint.Data.Constants.ControllerConstants.GetCurrentUser)]
    public User GetCurrent()
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);

        return Service.ReadByEmail(claim.Value);
    }
    
}