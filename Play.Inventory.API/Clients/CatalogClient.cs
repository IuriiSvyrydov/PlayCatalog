using Play.Inventory.API.Entities;

namespace Play.Inventory.API.Clients
{
    public class CatalogClient
    {
        private HttpClient _httpClient;

        public CatalogClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IReadOnlyCollection<InventoryItem>> GetCatalogItemAsync()
        {
            var items = await _httpClient.GetFromJsonAsync<IReadOnlyCollection<InventoryItem>>("/items");
            return items;
        }
    }
}
