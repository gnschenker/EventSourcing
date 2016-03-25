using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace ReadModelBuilder
{
    public class MongoDbProjectionWriter<TId, TView> : IProjectionWriter<TId, TView>
           where TView : class
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private static IMongoCollection<TView> _collection;

        public MongoDbProjectionWriter(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        public async Task Add(TView item)
        {
            var collection = GetCollection();
            await collection.InsertOneAsync(item);
        }

        public async Task Update(TId id, Action<TView> update)
        {
            var builder = Builders<TView>.Filter;
            var filter = builder.Eq("_id", id);
            var collection = GetCollection();
            var existingItem = await collection.Find(filter).FirstOrDefaultAsync();
            if (existingItem != null)
            {
                update(existingItem);
                await collection.ReplaceOneAsync(filter, existingItem);
            }

            throw new InvalidOperationException("Item does not exists");
        }

        private IMongoCollection<TView> GetCollection()
        {
            if (_collection != null) return _collection;
            var client = new MongoClient(_connectionString);
            var database = client.GetDatabase(_databaseName);
            _collection = database.GetCollection<TView>(typeof(TView).Name);
            return _collection;
        }
    }
}