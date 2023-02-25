using LuizaLabsApi.Models;
using LuizaLabsApi.Repositories.Interfaces;

namespace LuizaLabsApi.Repositories;

public class UserVerificationRepository : GenericRepository<UserVerification>, IUserVerificationRepository
{
    public UserVerificationRepository(DatabaseContext context, ILogger logger) : base(context, logger)
    { }
}