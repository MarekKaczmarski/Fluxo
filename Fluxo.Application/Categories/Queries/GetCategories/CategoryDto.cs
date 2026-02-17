namespace Fluxo.Application.Categories.Queries.GetCategories
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Icon { get; set; }
    }
}
