using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server.API;

public class AuthOptions
{
    public const string ISSUER = "Player.API";// издатель токена
    public const string AUDIENCE = "Players";// потребитель токена
    const string KEY = "SmallMediumBusiness SmallMediumBusiness";// ключ для шифрации
    public const int LIFETIME = 60 * 24;// время жизни токена в минутах

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}