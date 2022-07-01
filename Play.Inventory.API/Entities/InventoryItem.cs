using PlayCatalog.Model;

namespace Play.Inventory.API.Entities
{
    public class InventoryItem: IEntity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid CatalogItemId { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset AcquireDate { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
