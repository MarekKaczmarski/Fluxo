namespace Fluxo.Application.Common.Interfaces;

public interface ICategoryUniquenessChecker
{
    Task<bool> IsNameTakenAsync(string name, Guid? excludeId, CancellationToken ct);
}
