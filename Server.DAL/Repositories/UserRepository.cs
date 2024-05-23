using Joint.Data.Models;
using Server.DAL.Interfaces.Repositories;

namespace Server.DAL.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ServerContext context) : base(context)
    {
    }
}