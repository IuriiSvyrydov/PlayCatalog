using PlayCatalog.Model;

namespace PlayCatalog.API.Extensions
{
    public static class ItemExtension
    {
        public static ItemDto AddItem(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.DateCreated);
        }
    }
}
