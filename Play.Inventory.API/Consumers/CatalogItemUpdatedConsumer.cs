using MassTransit;
using Play.Inventory.API.Entities;
using PlayCatalog.Application.Repositories;
using PlayCatalog.Contracts;

namespace Play.Inventory.API.Consumers
{
    public class CatalogItemUpdatedConsumer : IConsumer<ItemCatalogUpdated>
    {
        private readonly IRepository<CatalogItem> _repository;

        public CatalogItemUpdatedConsumer(IRepository<CatalogItem> repository)
        {
            _repository = repository;
        }

        public async Task Consume(ConsumeContext<ItemCatalogUpdated> context)
        {
            var message = context.Message;
            var item = await _repository.GetItem(message.Id);
            if (item is  null)
            {
                item = new CatalogItem
                {
                    Id = message.Id,
                    Name = message.Name,
                    Description = message.Description,
                };
                await _repository.CreateAsync(item); ;
            }
            else
            {
                item.Name = message.Name;
                item.Description = message.Description;
                await _repository.UpdateAsync(item);
            }

           
        }
    }


}


