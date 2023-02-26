using LuizaLabsApi.Repositories.Interfaces;
using LuizaLabsApi.Repositories;

namespace LuizaLabsApi.Configuration;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly DatabaseContext _context;
    private readonly ILogger _logger;

    public IUserRepository User { get; private set; }
    public IUserVerificationRepository UserVerification { get; private set; }

    public UnitOfWork(DatabaseContext context, ILoggerFactory loggerFactory)
    {
        _context = context;
        _logger = loggerFactory.CreateLogger("logs");

        User = new UserRepository(_context, _logger);
        UserVerification = new UserVerificationRepository(_context, _logger);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
