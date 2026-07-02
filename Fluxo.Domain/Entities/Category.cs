using Fluxo.Domain.Exceptions;

namespace Fluxo.Domain.Entities
{
    public class Category
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; } = string.Empty;
        public string? Icon { get; private set; }

        private Category() { }

        public Category(Guid id, string name, string? icon)
        {
            Validate(id, name, icon);

            Id = id;
            Name = name.Trim();
            Icon = NormalizeIcon(icon);
        }

        public void Update(string name, string? icon)
        {
            Validate(Id, name, icon);

            Name = name.Trim();
            Icon = NormalizeIcon(icon);
        }

        private static string? NormalizeIcon(string? icon)
        {
            return string.IsNullOrWhiteSpace(icon) ? null : icon.Trim();
        }

        private static void Validate(Guid id, string name, string? icon)
        {
            if (id == Guid.Empty)
                throw new DomainException("Category ID is required.");

            if (string.IsNullOrWhiteSpace(name))
                throw new DomainException("Category name is required.");

            if (name.Length > 50)
                throw new DomainException("Category name cannot exceed 50 characters.");

            if (icon?.Length > 50)
                throw new DomainException("Icon name is too long.");
        }
    }
}
