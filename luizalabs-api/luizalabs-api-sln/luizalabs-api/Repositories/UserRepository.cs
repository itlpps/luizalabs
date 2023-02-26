using LuizaLabsApi.Models;
using LuizaLabsApi.Repositories.Interfaces;

namespace LuizaLabsApi.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(DatabaseContext context, ILogger logger)
        : base(context, logger) { }
}
