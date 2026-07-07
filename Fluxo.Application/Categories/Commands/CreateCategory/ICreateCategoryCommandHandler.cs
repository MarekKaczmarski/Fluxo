namespace Fluxo.Application.Categories.Commands.CreateCategory;

public interface ICreateCategoryCommandHandler
{
    Task<Guid> HandleAsync(CreateCategoryCommand command, CancellationToken ct);
}
