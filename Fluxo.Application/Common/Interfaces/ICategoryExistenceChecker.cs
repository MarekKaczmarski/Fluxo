namespace Fluxo.Application.Common.Interfaces;

public interface ICategoryExistenceChecker
{
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}
