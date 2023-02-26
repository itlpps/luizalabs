using LuizaLabsApi.Repositories.Interfaces;

namespace LuizaLabsApi.Configuration;

public interface IUnitOfWork
{
    IUserRepository User { get; }
    IUserVerificationRepository UserVerification { get; }
    void Dispose();
}
