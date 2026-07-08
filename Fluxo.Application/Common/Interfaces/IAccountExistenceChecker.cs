namespace Fluxo.Application.Common.Interfaces;

public interface IAccountExistenceChecker
{
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}
