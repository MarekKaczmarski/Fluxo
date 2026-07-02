namespace Fluxo.Application.Common.Interfaces
{
    public interface IAccountUniquenessChecker
    {
        Task<bool> IsNameTakenAsync(string name, Guid? excludeId, CancellationToken ct);
    }
}
