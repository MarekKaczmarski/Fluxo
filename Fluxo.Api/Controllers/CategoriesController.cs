using Fluxo.Application.Categories.Queries.GetCategories;
using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(IGetCategoriesHandler handler) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories(CancellationToken ct)
        {
            var result = await handler.HandleAsync(new GetCategoriesQuery(), ct);
            return Ok(result);
        }
    }
}
