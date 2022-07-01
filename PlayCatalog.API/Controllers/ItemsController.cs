using MassTransit;
using Microsoft.AspNetCore.Mvc;
using PlayCatalog.Application.Repositories;
using PlayCatalog.Contracts;
using PlayCatalog.Model;

namespace PlayCatalog.API.Controllers
{
    [Route("items")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IRepository<Item> _catalogRepository;
        private readonly IPublishEndpoint _publishEndpoint;


        public ItemsController(IRepository<Item> catalogRepository, IPublishEndpoint publishEndpoint)
        {
            _catalogRepository = catalogRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpGet]
        public async Task<IReadOnlyCollection<Item>> Get()
        {
            return await _catalogRepository.GetAllAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetById(Guid id)
        {
            var item = await _catalogRepository.GetItem(id);
            if (item is null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult<Item>> Create(CreateIemDto createIemDto)
        {
            var item = new Item
            {
                Name = createIemDto.Name,
                Description = createIemDto.Description,
                Price = createIemDto.Price,
                DateCreated = createIemDto.CreateDate
            };
            await _catalogRepository.CreateAsync(item);
            await _publishEndpoint.Publish(new ItemCatalogCreated(item.Id, item.Name, item.Description));
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);

        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Put(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = await _catalogRepository.GetItem(id);
            if (existingItem == null)
            {
                return NotFound();
            }
            existingItem.Name = itemDto.Name;
            existingItem.Description = itemDto.Description;
            existingItem.Price = itemDto.Price;
           await _catalogRepository.UpdateAsync(existingItem);
           await _publishEndpoint.Publish(new ItemCatalogUpdated(existingItem.Id, existingItem.Name, existingItem.Description));
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var item = await _catalogRepository.GetItem(id);
            if (item == null)
            {
                return NotFound();
            }

            await _catalogRepository.DeleteItem(item.Id);
            await _publishEndpoint.Publish(new ItemCatalogDeleted(item.Id));
            return NoContent();
        }
    }
}
