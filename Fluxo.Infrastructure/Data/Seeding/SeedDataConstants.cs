namespace Fluxo.Infrastructure.Data.Seeding;

public static class SeedDataConstants
{
    // Transactions
    public static readonly Guid Transaction1Id = Guid.Parse("7c9e6679-7425-40de-944b-e07fc1f90ae7");
    public static readonly Guid Transaction2Id = Guid.Parse("d290f1ee-6c89-4b20-bc5e-333333333333");
    public static readonly Guid Transaction3Id = Guid.Parse("a555e888-4444-4444-4444-111122223333");

    //Categories
    public static readonly Guid FoodCategoryId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479");
    public static readonly Guid TransferCategoryId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
    public static readonly Guid ElectronicsCategoryId = Guid.Parse("a1b2c3d4-e5f6-4a5b-bc6d-7e8f9a0b1c2d");
    public static readonly Guid PharmacyCategoryId = Guid.Parse("d4c3b2a1-f6e5-4b5a-ac6d-9f8e7d6c5b4a");
    public static readonly Guid TransportCategoryId = Guid.Parse("12345678-1234-1234-1234-123456789012");

    //Accounts (temporary account)
    public static readonly Guid DefaultAccountId = Guid.Parse("b2b2b2b2-b2b2-b2b2-b2b2-b2b2b2b2b2b2");
}
