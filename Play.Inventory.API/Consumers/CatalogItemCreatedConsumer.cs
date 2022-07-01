using MassTransit;
using Play.Inventory.API.Entities;
using PlayCatalog.Application.Repositories;
using PlayCatalog.Contracts;

namespace Play.Inventory.API.Consumers
{
    public class CatalogItemCreatedConsumer: IConsumer<ItemCatalogCreated>
    {
        private readonly  IRepository<CatalogItem> _repository;

        public CatalogItemCreatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ItemCatalogCreated> context)
        {
            var message = context.Message;
            var item =await _repository.GetItem(message.Id);
            if (item is not null)
            {
                return;
            }

            item = new CatalogItem
            {
                Id = message.Id,
                Name = message.Name,
                Description = message.Description,
            };
            await _repository.CreateAsync(item);
        }
    }
}
