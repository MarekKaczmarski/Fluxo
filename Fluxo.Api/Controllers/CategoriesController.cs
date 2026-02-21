using Fluxo.Application.Categories.Command;
using Fluxo.Application.Categories.Queries.GetCategories;
using Fluxo.Application.Transactions.Commands.CreateTransaction;
using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(
        IGetCategoriesHandler getHandler,
        ICreateCategoryHandler createHandler) 
        : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(CancellationToken ct)
        {
            var result = await getHandler.HandleAsync(new GetCategoriesQuery(), ct);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreateCategory(CreateCategoryCommand command, CancellationToken ct)
        {
            var id = await createHandler.HandleAsync(command, default);
            return Ok(id);
        }
    }
}
