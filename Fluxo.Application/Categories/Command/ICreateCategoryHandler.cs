using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Application.Categories.Command
{
    public interface ICreateCategoryHandler
    {
        Task<Guid> HandleAsync(CreateCategoryCommand command, CancellationToken ct);
    }
}
