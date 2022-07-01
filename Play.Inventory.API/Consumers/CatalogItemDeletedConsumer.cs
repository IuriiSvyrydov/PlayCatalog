using MassTransit;
using Play.Inventory.API.Entities;
using PlayCatalog.Application.Repositories;
using PlayCatalog.Contracts;

namespace Play.Inventory.API.Consumers
{
    public class CatalogItemDeletedConsumer: IConsumer<ItemCatalogDeleted>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemDeletedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ItemCatalogDeleted> context)
        {
            var message = context.Message;
            var item = await _repository.GetItem(message.Id);
            if (item!=null)
            {
                await _repository.DeleteItem(message.Id);
            }
        }
    }
}
