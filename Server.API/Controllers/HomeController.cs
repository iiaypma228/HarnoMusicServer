using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Joint.Data.Constants;
using Joint.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Server.API.Localize;
using Server.BLL.Interfaces;

namespace Server.API.Controllers;

public class HomeController : ControllerBase
{
    private readonly IUserService _service;
    
    public HomeController(IUserService service)
    {
        _service = service;
    }
    
    [AllowAnonymous]
    [HttpGet(ControllerConstants.PingRoute)]
    public object Ping()
    {
        System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
        var apiAssembly = new
        {
            ProductName = fvi.ProductName,
            VersionDatabase = fvi.ProductVersion,//bd version(расчетная версия БД)
            VersionAPI = fvi.FileVersion,//API(версия АПИ)
            CompanyName = fvi.CompanyName,
            Comments = fvi.Comments
        };
        var targetFramework = assembly.GetCustomAttributes(typeof(System.Runtime.Versioning.TargetFrameworkAttribute), false).FirstOrDefault() as System.Runtime.Versioning.TargetFrameworkAttribute;
        var result = $"Ping...{DateTime.Now} Ok({targetFramework?.FrameworkName})\n{apiAssembly}";
            
        return result;
    }

    [AllowAnonymous]
    [HttpPost(ControllerConstants.SignInRoute)]
    public object Auth([FromBody] User auth, int? tokenlifitime = null)
    {
        var isAuth = this._service.IsUserAuth(auth);

        if (isAuth == null)
        {
            throw new Exception(Resources.wrongAuth);
        }

        var res = this.Token(isAuth, tokenlifitime);

        return res;
    }
    
    [AllowAnonymous]
    [HttpPost(ControllerConstants.SignUpRoute)]
    public object SignUp([FromBody] User auth, int? tokenlifitime = null)
    {
        this._service.Save(auth);
        
        var res = this.Token(auth, tokenlifitime);

        return res;
    }
    
    private object Token(User user, int? lifeminutes = null)
    {
        
        var getIdentity = () => {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Email) };
            
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        };
        var identity = getIdentity();
        
        var now = DateTime.UtcNow;
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            claims:identity.Claims,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            expires: now.Add(TimeSpan.FromMinutes(lifeminutes != null && lifeminutes > 0 ? lifeminutes.Value : AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)

        );
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var result = new
        {
            access_token = encodedJwt,
            username = user.Username,
            expires = jwt.ValidTo
        };

        return result;
    }
}