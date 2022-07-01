
using Microsoft.AspNetCore.Mvc;
using Play.Inventory.API.Clients;
using Play.Inventory.API.Entities;
using PlayCatalog.Application.Repositories;

namespace Play.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<InventoryItem> _inventoryRepository;
        private readonly CatalogClient _catalogClient;
        public ItemsController(IRepository<InventoryItem> inventoryRepository, CatalogClient catalogClient)
        {
            _inventoryRepository = inventoryRepository;
            _catalogClient = catalogClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
        {
            if (userId == Guid.Empty) return BadRequest();
            var catalogItems = await _catalogClient.GetCatalogItemAsync();
            var inventoryItemEntities = await _inventoryRepository.
                GetAllAsync(item=>item.UserId==userId);
            var inventoryItemsDto = inventoryItemEntities.Select(inventoryItem =>
            {
                var catalogItem = catalogItems.Single(catalogItem => catalogItem.Id == inventoryItem.CatalogItemId);
                return inventoryItem.AsDto(catalogItem.Name, catalogItem.Description);
            });
            return Ok(inventoryItemsDto);
        }
        [HttpPost]
        public async Task<ActionResult> PostAsync(GrantItemsDto grantItemDto)
        {
            var inventoryItem = await _inventoryRepository.GetItem(x => x.UserId == grantItemDto.UserId
                                                                        && x.CatalogItemId ==
                                                                        grantItemDto.CatalogItemId);
            if (inventoryItem == null)
            {
                inventoryItem = new InventoryItem
                {
                    UserId = grantItemDto.UserId,
                    CatalogItemId = grantItemDto.CatalogItemId,
                    Quantity = grantItemDto.Quantity,
                    AcquireDate = DateTimeOffset.UtcNow
                };
                await _inventoryRepository.CreateAsync(inventoryItem);
            }
            else
            {
                inventoryItem.Quantity += grantItemDto.Quantity;
                await _inventoryRepository.UpdateAsync(inventoryItem);
            }

            return Ok();
        }
    }
}
