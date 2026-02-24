using System;
using System.Collections.Generic;
using System.Text;

namespace Fluxo.Application.Categories.Command.DeleteCategory
{
    public interface IDeleteCategoryHandler
    {
        Task HandleAsync(DeleteCategoryCommand command, CancellationToken ct);
    }
}
