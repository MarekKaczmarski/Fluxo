namespace Fluxo.Application.Categories.Commands.CreateCategory
{
    public interface ICreateCategoryHandler
    {
        Task<Guid> HandleAsync(CreateCategoryCommand command, CancellationToken ct);
    }
}
