using Microsoft.AspNetCore.Mvc;

namespace Fluxo.Api.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected ActionResult? EnsureMatchingId(Guid routeId, Guid bodyId)
        {
            if (routeId == bodyId)
                return null;

            return ValidationProblem(
                new ValidationProblemDetails(
                    new Dictionary<string, string[]>
                    {
                        ["id"] = ["ID in URL does not match ID in body."],
                    }
                )
            );
        }
    }
}
