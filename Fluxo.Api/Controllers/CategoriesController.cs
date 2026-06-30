using Fluxo.Application.Categories.Commands.CreateCategory;
using Fluxo.Application.Categories.Commands.DeleteCategory;
using Fluxo.Application.Categories.Commands.UpdateCategory;
using Fluxo.Application.Categories.Queries.GetCategories;
using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController(
        IGetCategoriesHandler getHandler,
        ICreateCategoryCommandHandler createHandler,
        IUpdateCategoryCommandHandler updateHandler,
        IDeleteCategoryCommandHandler deleteHandler
    ) : ApiControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(
            CancellationToken ct
        )
        {
            var result = await getHandler.HandleAsync(new GetCategoriesQuery(), ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCategory(
            CreateCategoryCommand command,
            CancellationToken ct
        )
        {
            var id = await createHandler.HandleAsync(command, ct);
            return CreatedAtAction(nameof(GetCategories), new { id }, id);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCategory(
            Guid id,
            UpdateCategoryCommand command,
            CancellationToken ct
        )
        {
            if (EnsureMatchingId(id, command.Id) is { } error)
                return error;

            await updateHandler.HandleAsync(command, ct);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
        {
            await deleteHandler.HandleAsync(new DeleteCategoryCommand(id), ct);
            return NoContent();
        }
    }
}
