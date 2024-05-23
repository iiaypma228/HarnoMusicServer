using Server.BLL.Interfaces;
using Joint.Data.Models;

namespace Server.BLL.Interfaces;

public interface IUserService : ICRUDService<User>
{
    User ReadByEmail(string email);
    User IsUserAuth(User user);
}