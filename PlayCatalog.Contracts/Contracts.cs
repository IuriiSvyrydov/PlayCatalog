

namespace PlayCatalog.Contracts
{
    public record ItemCatalogCreated(Guid Id, string Name, string Description);
    public record ItemCatalogUpdated(Guid Id, string Name, string Description);
    public record ItemCatalogDeleted(Guid Id);
}
