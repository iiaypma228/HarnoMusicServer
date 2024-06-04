using System.Security.Claims;
using Joint.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.BLL.Interfaces;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Server.API.Controllers;

[Authorize]
[Route("api/[controller]")]
public class UserController : BaseCRUDController<User, IUserService>
{
    private IHostingEnvironment hostingEnvironment;
    public UserController(IUserService service, IHostingEnvironment hostingEnvironment) : base(service)
    {
        this.hostingEnvironment = hostingEnvironment;
    }

    [HttpGet(Joint.Data.Constants.ControllerConstants.GetCurrentUser)]
    public User GetCurrent()
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);

        return Service.ReadByEmail(claim.Value);
    }

    [HttpPost("setAvatar")]
    public async Task<User> SetImage(IFormFile image)
    {
        var claim = User.Claims.FirstOrDefault(c=>c.Type == ClaimTypes.Name);

        var currentUser = Service.ReadByEmail(claim.Value);
        
        
        var uploadsPath = Path.Combine(hostingEnvironment.WebRootPath, "images");
        var filePath = Path.Combine(uploadsPath, Guid.NewGuid() + ".png");

        if (System.IO.File.Exists(currentUser.Avatar))
        {
            System.IO.File.Delete(currentUser.Avatar);
        }
        
        currentUser.Avatar = filePath.Replace(hostingEnvironment.WebRootPath, "");
        // Создайте папку, если она не существует
        if (!Directory.Exists(uploadsPath))
        {
            Directory.CreateDirectory(uploadsPath);
        }

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        Service.Save(currentUser);
        
        return currentUser;
    }
    
}