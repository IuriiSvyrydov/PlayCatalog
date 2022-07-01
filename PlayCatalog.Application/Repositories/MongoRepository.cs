using System.Linq.Expressions;
using MongoDB.Driver;
using PlayCatalog.Model;

namespace PlayCatalog.Application.Repositories
{
    public class MongoRepository<T> : IRepository<T> where T : IEntity
    {
        private readonly IMongoCollection<T> _dbItems;
        private readonly FilterDefinitionBuilder<T> _filter = Builders<T>.Filter;

        public MongoRepository(IMongoDatabase database, string collectionName)
        {
    
            _dbItems = database.GetCollection<T>(collectionName);
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync()
        {
            return await _dbItems.Find(_filter.Empty).ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbItems.Find(filter).ToListAsync();
        }

        public async Task<T> GetItem(Guid id)
        {
            FilterDefinition<T> filter = _filter.Eq(entity => id, id);
            return await _dbItems.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<T> GetItem(Expression<Func<T, bool>> filter)
        {
            return await _dbItems.Find(filter).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            await _dbItems.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            if (entity is null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            FilterDefinition<T> filter = _filter.Eq(existingEntity => existingEntity.Id, entity.Id);
                await _dbItems.ReplaceOneAsync(filter,entity);
        }

        public async Task DeleteItem(Guid id)
        {
            FilterDefinition<T> filter = _filter.Eq(entity => id, id);
            await _dbItems.DeleteOneAsync(filter);
        }

    }
}
